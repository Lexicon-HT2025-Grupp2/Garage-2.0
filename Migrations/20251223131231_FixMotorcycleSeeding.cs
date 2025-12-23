using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class FixMotorcycleSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ParkingSpot",
                keyColumn: "Id",
                keyValue: 2,
                column: "VehicleId",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ParkingSpot",
                keyColumn: "Id",
                keyValue: 2,
                column: "VehicleId",
                value: -2);
        }
    }
}
