﻿namespace L1.Data.Dtos.Hotels
{
    public record HotelDto(string Name, string Address, string PhoneNumber);
    public record CreateHotelDto(string Name, string Address, string PhoneNumber);
    public record UpdateHotelDto(string Name, string Address, string PhoneNumber);
}
