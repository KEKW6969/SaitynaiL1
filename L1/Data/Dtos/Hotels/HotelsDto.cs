namespace L1.Data.Dtos.Hotels
{
    public record HotelDto(int Id, string Name, string Address, string PhoneNumber, string UserId);
    public record CreateHotelDto(string Name, string Address, string PhoneNumber);
    public record UpdateHotelDto(string Name, string Address, string PhoneNumber);
}
