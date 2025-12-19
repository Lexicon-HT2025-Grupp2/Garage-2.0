using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ParkedVehicle",
                columns: new[] { "RegistrationNumber", "ArrivalTime", "Brand", "Color", "Model", "Note", "NumberOfWheels", "Type" },
                values: new object[,]
                {
                    { "ABC123", new DateTime(2025, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), "Volvo", "Blue", "XC60", "Test car", 4, "Car" },
                    { "JKL456", new DateTime(2025, 12, 17, 14, 30, 0, 0, DateTimeKind.Unspecified), "Toyota", "Red", "Corolla", "Compact car", 4, "Car" },
                    { "MNO321", new DateTime(2025, 12, 16, 8, 15, 0, 0, DateTimeKind.Unspecified), "Scania", "White", "R500", "Heavy duty truck", 6, "Truck" },
                    { "XYZ789", new DateTime(2025, 12, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), "BMW", "Black", "R1250", "Test motorcycle", 2, "Motorcycle" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "RegistrationNumber",
                keyValue: "ABC123");

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "RegistrationNumber",
                keyValue: "JKL456");

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "RegistrationNumber",
                keyValue: "MNO321");

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "RegistrationNumber",
                keyValue: "XYZ789");
        }
    }
}
