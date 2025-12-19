using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class SeedWithNegativeIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "ParkedVehicle",
                columns: new[] { "Id", "ArrivalTime", "Brand", "Color", "Model", "Note", "NumberOfWheels", "RegistrationNumber", "Type" },
                values: new object[,]
                {
                    { -4, new DateTime(2025, 12, 16, 8, 15, 0, 0, DateTimeKind.Unspecified), "Scania", "White", "R500", "Heavy duty truck", 6, "MNO321", "Truck" },
                    { -3, new DateTime(2025, 12, 17, 14, 30, 0, 0, DateTimeKind.Unspecified), "Toyota", "Red", "Corolla", "Compact car", 4, "JKL456", "Car" },
                    { -2, new DateTime(2025, 12, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), "BMW", "Black", "R1250", "Test motorcycle", 2, "XYZ789", "Motorcycle" },
                    { -1, new DateTime(2025, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), "Volvo", "Blue", "XC60", "Test car", 4, "ABC123", "Car" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.InsertData(
                table: "ParkedVehicle",
                columns: new[] { "Id", "ArrivalTime", "Brand", "Color", "Model", "Note", "NumberOfWheels", "RegistrationNumber", "Type" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), "Volvo", "Blue", "XC60", "Test car", 4, "ABC123", "Car" },
                    { 2, new DateTime(2025, 12, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), "BMW", "Black", "R1250", "Test motorcycle", 2, "XYZ789", "Motorcycle" },
                    { 3, new DateTime(2025, 12, 17, 14, 30, 0, 0, DateTimeKind.Unspecified), "Toyota", "Red", "Corolla", "Compact car", 4, "JKL456", "Car" },
                    { 4, new DateTime(2025, 12, 16, 8, 15, 0, 0, DateTimeKind.Unspecified), "Scania", "White", "R500", "Heavy duty truck", 6, "MNO321", "Truck" }
                });
        }
    }
}
