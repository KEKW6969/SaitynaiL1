using L1.Auth.Model;
using L1.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace L1.Data
{
    public class HotelsDbContext : IdentityDbContext<HotelRestUser>
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:l1dbserver.database.windows.net,1433;Initial Catalog=L1_db;Persist Security Info=False;User ID=mirtisnerablogai;Password=nuzudykmane123.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
