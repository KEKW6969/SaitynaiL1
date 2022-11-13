using L1.Auth.Model;
using L1.Data.Dtos.Floors;
using L1.Data.Dtos.Rooms;
using L1.Data.Entities;
using L1.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L1.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/hotels/{hotelId}/floors/{floorId}/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsRepository _roomsRepository;
        private readonly IFloorsRepository _floorsRepository;
        private readonly IHotelsRepository _hotelsRepository;
        private readonly IAuthorizationService _authorizationService;

        public RoomsController(IFloorsRepository floorsRepository, IHotelsRepository hotelsRepository, IRoomsRepository roomsRepository, IAuthorizationService authorizationService)
        {
            _floorsRepository = floorsRepository;
            _hotelsRepository = hotelsRepository;
            _roomsRepository = roomsRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetMany(int hotelId, int floorId)
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

            var rooms = await _roomsRepository.GetManyAsync(floor);
            return Ok(rooms.Select(o => new RoomDto(o.Id, o.Number, o.Price, o.Description, o.Floor)));
        }

        [HttpGet()]
        [Route("{roomId}", Name = "RoomFloor")]
        public async Task<ActionResult<RoomDto>> Get(int hotelId, int floorId, int roomId)
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

            var room = await _roomsRepository.GetAsync(floor, roomId);

            if (room == null)
                return NotFound();

            return new RoomDto(room.Id, room.Number, room.Price, room.Description, room.Floor);
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> Create(int hotelId, int floorId, CreateRoomDto createRoomDto)
        {
            var hotel = _hotelsRepository.GetAsync(hotelId);

            if (hotel == null || hotel.Result == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, hotel, PolicyNames.ResourceOwner);

            if (!authorizationResult.Succeeded)
                return Forbid();

            var floor = _floorsRepository.GetAsync(hotel.Result, floorId);

            if (floor == null || floor.Result == null)
                return NotFound();

            var room = new Room { Number = createRoomDto.Number, Price = createRoomDto.Price, Description = createRoomDto.Description, Floor = floor.Result};

            await _roomsRepository.CreateAsync(room);

            return Created("", new RoomDto(room.Id, room.Number, room.Price, room.Description, room.Floor));
        }

        [HttpPut]
        [Route("{roomId}")]
        public async Task<ActionResult<RoomDto>> Update(int hotelId, int floorId, int roomId, UpdateRoomDto updateRoomDto)
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

            var room = await _roomsRepository.GetAsync(floor, roomId);

            if (room == null)
                return NotFound();

            room.Number = updateRoomDto.Number;
            room.Price = updateRoomDto.Price;
            room.Description = updateRoomDto.Description;

            await _roomsRepository.UpdateAsync(room);

            return Ok(new RoomDto(room.Id, room.Number, room.Price, room.Description, floor));
        }

        [HttpDelete]
        [Route("{roomId}")]
        public async Task<ActionResult> Remove(int hotelId, int floorId, int roomId)
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

            var room = await _roomsRepository.GetAsync(floor, roomId);

            if (room == null)
                return NotFound();

            await _roomsRepository.DeleteAsync(room);

            return NoContent();
        }
    }
}
