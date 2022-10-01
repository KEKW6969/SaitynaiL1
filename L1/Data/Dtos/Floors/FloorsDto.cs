using L1.Data.Entities;
namespace L1.Data.Dtos.Floors
{
    public record FloorDto(int Id, int Number, Hotel Hotel);
    public record CreateFloorDto(int Number);
    public record UpdateFloorDto(int Number);
}
