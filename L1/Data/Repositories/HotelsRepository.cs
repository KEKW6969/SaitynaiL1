using L1.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace L1.Data.Repositories
{
    public interface IHotelsRepository
    {
        Task CreateAsync(Hotel hotel);
        Task DeleteAsync(Hotel hotel);
        Task<Hotel?> GetAsync(int hotelId);
        Task<IReadOnlyList<Hotel>> GetManyAsync();
        Task UpdateAsync(Hotel hotel);
    }

    public class HotelsRepository : IHotelsRepository
    {
        private readonly HotelsDbContext _hotelsDbContext;

        public HotelsRepository(HotelsDbContext hotelsDbContext)
        {
            _hotelsDbContext = hotelsDbContext;
        }

        public async Task<Hotel?> GetAsync(int hotelId)
        {
            return await _hotelsDbContext.Hotels.FirstOrDefaultAsync(o => o.Id == hotelId);
        }

        public async Task<IReadOnlyList<Hotel>> GetManyAsync()
        {
            return await _hotelsDbContext.Hotels.ToListAsync();
        }

        public async Task CreateAsync(Hotel hotel)
        {
            _hotelsDbContext.Hotels.Add(hotel);
            await _hotelsDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Hotel hotel)
        {
            _hotelsDbContext.Hotels.Update(hotel);
            await _hotelsDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Hotel hotel)
        {
            _hotelsDbContext.Hotels.Remove(hotel);
            await _hotelsDbContext.SaveChangesAsync();
        }


    }
}
