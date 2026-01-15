using Garage_2._0.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Data
{
    public class Garage_2_0Context : IdentityDbContext<ApplicationUser>
    {
        public Garage_2_0Context(DbContextOptions<Garage_2_0Context> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; } = default!;
        public DbSet<VehicleType> VehicleTypes { get; set; } = default!;
        public DbSet<ParkingSpot> ParkingSpots { get; set; } = default!;
        public DbSet<Parking> Parkings { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
           .HasIndex(u => u.Personnummer)
           .IsUnique();

            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.RegistrationNumber)
                .IsUnique();

        }
    }
}
