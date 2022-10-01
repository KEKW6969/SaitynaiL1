using L1.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace L1.Data.Repositories
{
    public interface IRoomsRepository
    {
        Task CreateAsync(Room room);
        Task DeleteAsync(Room room);
        Task<Room?> GetAsync(Floor floor, int roomId);
        Task<IReadOnlyList<Room>> GetManyAsync(Floor floor);
        Task UpdateAsync(Room room);
    }

    public class RoomsRepository : IRoomsRepository
    {
        private readonly HotelsDbContext _hotelsDbContext;

        public RoomsRepository(HotelsDbContext hotelsDbContext)
        {
            _hotelsDbContext = hotelsDbContext;
        }

        public async Task<Room?> GetAsync(Floor floor, int roomId)
        {
            return await _hotelsDbContext.Rooms.Where(o => o.Floor == floor).FirstOrDefaultAsync(o => o.Id == roomId);
        }

        public async Task<IReadOnlyList<Room>> GetManyAsync(Floor floor)
        {
            return await _hotelsDbContext.Rooms.Where(o => o.Floor == floor).ToListAsync();
        }

        public async Task CreateAsync(Room room)
        {
            _hotelsDbContext.Rooms.Add(room);
            await _hotelsDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room)
        {
            _hotelsDbContext.Rooms.Update(room);
            await _hotelsDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Room room)
        {
            _hotelsDbContext.Rooms.Remove(room);
            await _hotelsDbContext.SaveChangesAsync();
        }


    }
}
