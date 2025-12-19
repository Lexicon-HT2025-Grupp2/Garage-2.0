using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
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
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkedVehicle", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ParkedVehicle",
                columns: new[] { "Id", "ArrivalTime", "Brand", "Color", "Model", "Note", "NumberOfWheels", "RegistrationNumber", "Type" },
                values: new object[,]
                {
                    { -4, new DateTime(2025, 12, 16, 8, 15, 0, 0, DateTimeKind.Unspecified), "Scania", "White", "R500", "Heavy duty truck", 6, "MNO321", 6 },
                    { -3, new DateTime(2025, 12, 17, 14, 30, 0, 0, DateTimeKind.Unspecified), "Toyota", "Red", "Corolla", "Compact car", 4, "JKL456", 0 },
                    { -2, new DateTime(2025, 12, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), "BMW", "Black", "R1250", "Test motorcycle", 2, "XYZ789", 1 },
                    { -1, new DateTime(2025, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), "Volvo", "Blue", "XC60", "Test car", 4, "ABC123", 0 }
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
