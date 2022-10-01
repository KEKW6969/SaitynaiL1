using L1.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace L1.Data.Repositories
{
    public interface IFloorsRepository
    {
        Task CreateAsync(Floor floor);
        Task DeleteAsync(Floor floor);
        Task<Floor?> GetAsync(Hotel hotel, int floorId);
        Task<IReadOnlyList<Floor>> GetManyAsync(Hotel hotel);
        Task UpdateAsync(Floor floor);
    }

    public class FloorsRepository : IFloorsRepository
    {
        private readonly HotelsDbContext _hotelsDbContext;

        public FloorsRepository(HotelsDbContext hotelsDbContext)
        {
            _hotelsDbContext = hotelsDbContext;
        }

        public async Task<Floor?> GetAsync(Hotel hotel, int floorId)
        {
            return await _hotelsDbContext.Floors.Where(o => o.Hotel == hotel).FirstOrDefaultAsync(o => o.Id == floorId);
        }

        public async Task<IReadOnlyList<Floor>> GetManyAsync(Hotel hotel)
        {
            return await _hotelsDbContext.Floors.Where(o => o.Hotel == hotel).ToListAsync();
        }

        public async Task CreateAsync(Floor floor)
        {
            _hotelsDbContext.Floors.Add(floor);
            await _hotelsDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Floor floor)
        {
            _hotelsDbContext.Floors.Update(floor);
            await _hotelsDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Floor floor)
        {
            _hotelsDbContext.Floors.Remove(floor);
            await _hotelsDbContext.SaveChangesAsync();
        }


    }
}
