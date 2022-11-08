using L1.Auth.Model;
using L1.Data.Dtos.Hotels;
using L1.Data.Entities;
using L1.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace L1.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsRepository _hotelsRepository;

        public HotelsController(IHotelsRepository hotelsRepository)
        {
            _hotelsRepository = hotelsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<HotelDto>> GetMany()
        {
            var hotels = await _hotelsRepository.GetManyAsync();

            return hotels.Select(o => new HotelDto(o.Id, o.Name, o.Address, o.PhoneNumber));
        }

        [HttpGet()]
        [Route("{hotelId}", Name = "GetHotel")]
        public async Task<ActionResult<HotelDto>> Get(int hotelId)
        {
            var hotel = await _hotelsRepository.GetAsync(hotelId);

            if (hotel == null)
                return NotFound();

            return new HotelDto(hotel.Id, hotel.Name, hotel.Address, hotel.PhoneNumber);
        }

        [HttpPost]
        [Authorize(Roles = HotelRoles.HotelUser)]
        public async Task<ActionResult<HotelDto>> Create(CreateHotelDto createHotelDto)
        {
            var hotel = new Hotel { Name = createHotelDto.Name, Address = createHotelDto.Address, PhoneNumber = createHotelDto.PhoneNumber,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) };

            await _hotelsRepository.CreateAsync(hotel);

            return Created("", new HotelDto(hotel.Id, hotel.Name, hotel.Address, hotel.PhoneNumber));
        }

        [HttpPut]
        [Route("{hotelId}")]
        [Authorize(Roles = HotelRoles.HotelUser)]
        public async Task<ActionResult<HotelDto>> Update(int hotelId, UpdateHotelDto updateHotelDto)
        {
            var hotel = await _hotelsRepository.GetAsync(hotelId);

            if (hotel == null)
                return NotFound();

            hotel.Name = updateHotelDto.Name;
            hotel.Address = updateHotelDto.Address;
            hotel.PhoneNumber = updateHotelDto.PhoneNumber;

            await _hotelsRepository.UpdateAsync(hotel);

            return Ok(new HotelDto(hotel.Id, hotel.Name, hotel.Address, hotel.PhoneNumber));
        }

        [HttpDelete]
        [Route("{hotelId}")]
        public async Task<ActionResult> Remove(int hotelId)
        {
            var hotel = await _hotelsRepository.GetAsync(hotelId);

            if (hotel == null)
                return NotFound();

            await _hotelsRepository.DeleteAsync(hotel);

            return NoContent();
        }
    }
}
