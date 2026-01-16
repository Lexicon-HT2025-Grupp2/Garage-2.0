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
        public DbSet<ParkingSpot> ParkingSpotsTypes { get; set; } = default!;
        public DbSet<Parking> Parkings { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
           .HasIndex(u => u.Personnummer)
           .IsUnique();

            // Ensure RegistrationNumber is unique
            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.RegistrationNumber)
                .IsUnique();

            // Configure Vehicle → Owner (ApplicationUser) relation
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Owner)
                .WithMany(u => u.Vehicles) // Vehicles must exist in ApplicationUser
                .HasForeignKey(v => v.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Optional: configure VehicleType relation
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.VehicleType)
                .WithMany()
                .HasForeignKey(v => v.VehicleTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ParkingSpotType>(b =>
            {
                b.HasKey(t => new { t.ParkingSpotId, t.VehicleTypeId });
                b.HasOne(t => t.PSpot).WithMany(s => s.PTypes);
                b.HasOne(t => t.VehicleType).WithMany(v => v.PTypes);
            });
            modelBuilder.Entity<ParkingSpot>()
                .HasOne(s => s.Parent).WithMany(s => s.SubSpots).HasForeignKey(s => s.ParentId);
        }
    }
}
