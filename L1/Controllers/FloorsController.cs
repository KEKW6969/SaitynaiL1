using L1.Auth.Model;
using L1.Data.Dtos.Floors;
using L1.Data.Entities;
using L1.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L1.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/hotels/{hotelId}/floors")]
    public class FloorsController : ControllerBase
    {
        private readonly IFloorsRepository _floorsRepository;
        private readonly IHotelsRepository _hotelsRepository;
        private readonly IAuthorizationService _authorizationService;

        public FloorsController(IFloorsRepository floorsRepository, IHotelsRepository hotelsRepository, IAuthorizationService authorizationService)
        {
            _floorsRepository = floorsRepository;
            _hotelsRepository = hotelsRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FloorDto>>> GetMany(int hotelId)
        {
            var hotel = await _hotelsRepository.GetAsync(hotelId);

            if (hotel == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, hotel, PolicyNames.ResourceOwner);

            if (!authorizationResult.Succeeded)
                return Forbid();

            var floors = await _floorsRepository.GetManyAsync(hotel);
            return Ok(floors.Select(o => new FloorDto(o.Id, o.Number, o.Hotel)));
        }

        [HttpGet()]
        [Route("{floorId}", Name = "GetFloor")]
        public async Task<ActionResult<FloorDto>> Get(int hotelId, int floorId)
        {
            var hotel = await _hotelsRepository.GetAsync(hotelId);

            if (hotel == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, hotel, PolicyNames.ResourceOwner);

            if (!authorizationResult.Succeeded)
                return Forbid();

            var floor = await _floorsRepository.GetAsync(hotel, floorId);

            if (floor == null)
                return NotFound();

            return new FloorDto(floor.Id, floor.Number, floor.Hotel);
        }

        [HttpPost]
        public async Task<ActionResult<FloorDto>> Create(int hotelId, CreateFloorDto createFloorDto)
        {
            var hotel = _hotelsRepository.GetAsync(hotelId);

            if (hotel == null || hotel.Result == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, hotel.Result, PolicyNames.ResourceOwner);

            if (!authorizationResult.Succeeded)
                return Forbid();

            var floor = new Floor { Number = createFloorDto.Number, Hotel = hotel.Result };

            await _floorsRepository.CreateAsync(floor);

            return Created("", new FloorDto(floor.Id, floor.Number, floor.Hotel));
        }

        [HttpPut]
        [Route("{floorId}")]
        public async Task<ActionResult<FloorDto>> Update(int hotelId, int floorId, UpdateFloorDto updateFloorDto)
        {
            var hotel = await _hotelsRepository.GetAsync(hotelId);

            if (hotel == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, hotel, PolicyNames.ResourceOwner);

            if (!authorizationResult.Succeeded)
                return Forbid();

            var floor = await _floorsRepository.GetAsync(hotel, floorId);

            if (floor == null)
                return NotFound();

            floor.Number = updateFloorDto.Number;

            await _floorsRepository.UpdateAsync(floor);

            return Ok(new FloorDto(floor.Id, floor.Number, hotel));
        }

        [HttpDelete]
        [Route("{floorId}")]
        public async Task<ActionResult> Remove(int hotelId, int floorId)
        {
            var hotel = await _hotelsRepository.GetAsync(hotelId);

            if (hotel == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, hotel, PolicyNames.ResourceOwner);

            if (!authorizationResult.Succeeded)
                return Forbid();

            var floor = await _floorsRepository.GetAsync(hotel, floorId);

            if (floor == null)
                return NotFound();

            await _floorsRepository.DeleteAsync(floor);

            return NoContent();
        }
    }
}
