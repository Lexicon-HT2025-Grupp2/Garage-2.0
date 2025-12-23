using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class InitClean : Migration
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
                    SpotNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkedVehicle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSpot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SpotNumber = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: true),
                    MotorcycleCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingSpot_ParkedVehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "ParkedVehicle",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ParkedVehicle",
                columns: new[] { "Id", "ArrivalTime", "Brand", "Color", "Model", "Note", "NumberOfWheels", "RegistrationNumber", "SpotNumber", "Type" },
                values: new object[,]
                {
                    { -4, new DateTime(2025, 12, 16, 8, 15, 0, 0, DateTimeKind.Unspecified), "Scania", "White", "R500", "Heavy duty truck", 6, "MNO321", 4, 6 },
                    { -3, new DateTime(2025, 12, 17, 14, 30, 0, 0, DateTimeKind.Unspecified), "Toyota", "Red", "Corolla", "Compact car", 4, "JKL456", 3, 0 },
                    { -2, new DateTime(2025, 12, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), "BMW", "Black", "R1250", "Test motorcycle", 2, "XYZ789", 2, 1 },
                    { -1, new DateTime(2025, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), "Volvo", "Blue", "XC60", "Test car", 4, "ABC123", 1, 0 }
                });

            migrationBuilder.InsertData(
                table: "ParkingSpot",
                columns: new[] { "Id", "MotorcycleCount", "SpotNumber", "VehicleId" },
                values: new object[,]
                {
                    { 6, 0, 6, null },
                    { 7, 0, 7, null },
                    { 8, 0, 8, null },
                    { 9, 0, 9, null },
                    { 10, 0, 10, null },
                    { 11, 0, 11, null },
                    { 12, 0, 12, null },
                    { 13, 0, 13, null },
                    { 14, 0, 14, null },
                    { 15, 0, 15, null },
                    { 16, 0, 16, null },
                    { 17, 0, 17, null },
                    { 18, 0, 18, null },
                    { 19, 0, 19, null },
                    { 20, 0, 20, null },
                    { 1, 0, 1, -1 },
                    { 2, 1, 2, -2 },
                    { 3, 0, 3, -3 },
                    { 4, 0, 4, -4 },
                    { 5, 0, 5, -4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpot_VehicleId",
                table: "ParkingSpot",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingSpot");

            migrationBuilder.DropTable(
                name: "ParkedVehicle");
        }
    }
}
