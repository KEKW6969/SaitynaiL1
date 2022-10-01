﻿using L1.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace L1.Data
{
    public class HotelsDbContext : DbContext
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=ForumDb2");
        }
    }
}