using Microsoft.EntityFrameworkCore;
using TruckScheduler.Models;

namespace TruckScheduler.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<ParkingBay> ParkingBays { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }

    }
}
