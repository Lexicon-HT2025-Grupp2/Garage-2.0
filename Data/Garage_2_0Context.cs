using Garage_2._0.Models;
using Microsoft.AspNetCore.Identity;
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

        public DbSet<Garage_2._0.Models.ParkedVehicle> ParkedVehicle { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data with negative IDs to avoid conflicts with auto-generated IDs
            modelBuilder.Entity<ParkedVehicle>().HasData(
                new ParkedVehicle
                {
                    Id = -1,
                    Type = VehicleType.Car,
                    RegistrationNumber = "ABC123",
                    Color = "Blue",
                    Brand = "Volvo",
                    Model = "XC60",
                    NumberOfWheels = 4,
                    ArrivalTime = new DateTime(2025, 12, 18, 9, 0, 0),
                    ParkingSpots = "1"
                },
                new ParkedVehicle
                {
                    Id = -2,
                    Type = VehicleType.Motorcycle,
                    RegistrationNumber = "XYZ789",
                    Color = "Black",
                    Brand = "BMW",
                    Model = "R1250",
                    NumberOfWheels = 2,
                    ArrivalTime = new DateTime(2025, 12, 18, 10, 0, 0),
                    ParkingSpots = "5,A"
                },
                new ParkedVehicle
                {
                    Id = -3,
                    Type = VehicleType.Car,
                    RegistrationNumber = "JKL456",
                    Color = "Red",
                    Brand = "Toyota",
                    Model = "Corolla",
                    NumberOfWheels = 4,
                    ArrivalTime = new DateTime(2025, 12, 17, 14, 30, 0),
                    ParkingSpots = "3"
                },
                new ParkedVehicle
                {
                    Id = -4,
                    Type = VehicleType.Truck,
                    RegistrationNumber = "MNO321",
                    Color = "White",
                    Brand = "Scania",
                    Model = "R500",
                    NumberOfWheels = 6,
                    ArrivalTime = new DateTime(2025, 12, 16, 8, 15, 0),
                    ParkingSpots = "10,11"
                },
                new ParkedVehicle
                {
                    Id = -5,
                    Type = VehicleType.Bus,
                    RegistrationNumber = "BUS999",
                    Color = "Yellow",
                    Brand = "Mercedes",
                    Model = "Citaro",
                    NumberOfWheels = 6,
                    ArrivalTime = new DateTime(2025, 12, 17, 8, 0, 0),
                    ParkingSpots = "15"
                },
                new ParkedVehicle
                {
                    Id = -6,
                    Type = VehicleType.Motorcycle,
                    RegistrationNumber = "MC555",
                    Color = "Red",
                    Brand = "Harley Davidson",
                    Model = "Street 750",
                    NumberOfWheels = 2,
                    ArrivalTime = new DateTime(2025, 12, 18, 11, 30, 0),
                    ParkingSpots = "5,B"
                },
                new ParkedVehicle
                {
                    Id = -7,
                    Type = VehicleType.Car,
                    RegistrationNumber = "DEF789",
                    Color = "Silver",
                    Brand = "Audi",
                    Model = "A4",
                    NumberOfWheels = 4,
                    ArrivalTime = new DateTime(2025, 12, 18, 13, 15, 0),
                    ParkingSpots = "7"
                },
                new ParkedVehicle
                {
                    Id = -8,
                    Type = VehicleType.Boat,
                    RegistrationNumber = "BOAT123",
                    Color = "White",
                    Brand = "Yamaha",
                    Model = "242X",
                    NumberOfWheels = 0,
                    ArrivalTime = new DateTime(2025, 12, 15, 10, 0, 0),
                    ParkingSpots = "20,21,22"
                }
            );
        }
    }
}
