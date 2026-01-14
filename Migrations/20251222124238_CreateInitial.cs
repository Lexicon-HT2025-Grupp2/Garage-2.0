using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class CreateInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkedVehicle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumberOfWheels = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParkingSpots = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkedVehicle", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ParkedVehicle",
                columns: new[] { "Id", "ArrivalTime", "Brand", "Color", "Model", "Note", "NumberOfWheels", "ParkingSpots", "RegistrationNumber", "Type" },
                values: new object[,]
                {
                    { -8, new DateTime(2025, 12, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), "Yamaha", "White", "242X", "", 0, "20,21,22", "BOAT123", 4 },
                    { -7, new DateTime(2025, 12, 18, 13, 15, 0, 0, DateTimeKind.Unspecified), "Audi", "Silver", "A4", "", 4, "7", "DEF789", 0 },
                    { -6, new DateTime(2025, 12, 18, 11, 30, 0, 0, DateTimeKind.Unspecified), "Harley Davidson", "Red", "Street 750", "", 2, "5,B", "MC555", 1 },
                    { -5, new DateTime(2025, 12, 17, 8, 0, 0, 0, DateTimeKind.Unspecified), "Mercedes", "Yellow", "Citaro", "", 6, "15", "BUS999", 2 },
                    { -4, new DateTime(2025, 12, 16, 8, 15, 0, 0, DateTimeKind.Unspecified), "Scania", "White", "R500", "", 6, "10,11", "MNO321", 6 },
                    { -3, new DateTime(2025, 12, 17, 14, 30, 0, 0, DateTimeKind.Unspecified), "Toyota", "Red", "Corolla", "", 4, "3", "JKL456", 0 },
                    { -2, new DateTime(2025, 12, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), "BMW", "Black", "R1250", "", 2, "5,A", "XYZ789", 1 },
                    { -1, new DateTime(2025, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), "Volvo", "Blue", "XC60", "", 4, "1", "ABC123", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkedVehicle");
        }
    }
}
