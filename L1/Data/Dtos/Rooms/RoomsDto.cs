using L1.Data.Entities;
namespace L1.Data.Dtos.Rooms
{
    public record RoomDto(int Id, int Number, double Price, string Description, Floor Floor);
    public record CreateRoomDto(int Number, double Price, string Description);
    public record UpdateRoomDto(int Number, double Price, string Description);
}
