using Garage_2._0.Models;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Data
{
    public class Garage_2_0Context : DbContext
    {
        public Garage_2_0Context (DbContextOptions<Garage_2_0Context> options)
            : base(options)
        {
        }

        public DbSet<Garage_2._0.Models.ParkedVehicle> ParkedVehicle { get; set; } = default!;
        public DbSet<ParkingSpot> ParkingSpot { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Disable auto-generation for seeded IDs
            modelBuilder.Entity<ParkingSpot>().Property(p => p.Id).ValueGeneratedNever();

            // ---------------------------------------------
            // Seed data for ParkedVehicle (example vehicles)
            // ---------------------------------------------
            modelBuilder.Entity<ParkedVehicle>().HasData(
                new ParkedVehicle
                {
                    Id = -1,
                    RegistrationNumber = "ABC123",
                    Type = VehicleType.Car,
                    Color = "Blue",
                    Brand = "Volvo",
                    Model = "XC60",
                    NumberOfWheels = 4,
                    ArrivalTime = new DateTime(2025, 12, 18, 9, 0, 0),
                    Note = "Test car",
                    SpotNumber = 1
                },
                new ParkedVehicle
                {
                    Id = -2,
                    RegistrationNumber = "XYZ789",
                    Type = VehicleType.Motorcycle,
                    Color = "Black",
                    Brand = "BMW",
                    Model = "R1250",
                    NumberOfWheels = 2,
                    ArrivalTime = new DateTime(2025, 12, 18, 10, 0, 0),
                    Note = "Test motorcycle",
                    SpotNumber = 2
                },
                new ParkedVehicle
                {
                    Id = -3,
                    RegistrationNumber = "JKL456",
                    Type = VehicleType.Car,
                    Color = "Red",
                    Brand = "Toyota",
                    Model = "Corolla",
                    NumberOfWheels = 4,
                    ArrivalTime = new DateTime(2025, 12, 17, 14, 30, 0),
                    Note = "Compact car",
                    SpotNumber = 3
                },
                new ParkedVehicle
                {
                    Id = -4,
                    RegistrationNumber = "MNO321",
                    Type = VehicleType.Truck,
                    Color = "White",
                    Brand = "Scania",
                    Model = "R500",
                    NumberOfWheels = 6,
                    ArrivalTime = new DateTime(2025, 12, 16, 8, 15, 0),
                    Note = "Heavy duty truck",
                    SpotNumber = 4
                }
            );

            // ------------------------------------------------------
            // Seed data for ParkingSpot (20 fixed parking spots)
            // ------------------------------------------------------
            modelBuilder.Entity<ParkingSpot>().HasData(
    new ParkingSpot { Id = 1, SpotNumber = 1, VehicleId = -1, MotorcycleCount = 0 },
   new ParkingSpot { Id = 2, SpotNumber = 2, VehicleId = null, MotorcycleCount = 1 },
    new ParkingSpot { Id = 3, SpotNumber = 3, VehicleId = -3, MotorcycleCount = 0 },
    new ParkingSpot { Id = 4, SpotNumber = 4, VehicleId = -4, MotorcycleCount = 0 },
    new ParkingSpot { Id = 5, SpotNumber = 5, VehicleId = -4, MotorcycleCount = 0 },
    new ParkingSpot { Id = 6, SpotNumber = 6, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 7, SpotNumber = 7, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 8, SpotNumber = 8, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 9, SpotNumber = 9, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 10, SpotNumber = 10, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 11, SpotNumber = 11, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 12, SpotNumber = 12, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 13, SpotNumber = 13, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 14, SpotNumber = 14, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 15, SpotNumber = 15, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 16, SpotNumber = 16, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 17, SpotNumber = 17, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 18, SpotNumber = 18, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 19, SpotNumber = 19, VehicleId = null, MotorcycleCount = 0 },
    new ParkingSpot { Id = 20, SpotNumber = 20, VehicleId = null, MotorcycleCount = 0 }
);

        }
    }
}
