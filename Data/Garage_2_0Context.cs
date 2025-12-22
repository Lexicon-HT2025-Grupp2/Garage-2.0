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
                    Note = "Test car"
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
                    Note = "Test motorcycle"
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
                    Note = "Compact car"
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
                    Note = "Heavy duty truck"
                }

            );
        }
    }
}
