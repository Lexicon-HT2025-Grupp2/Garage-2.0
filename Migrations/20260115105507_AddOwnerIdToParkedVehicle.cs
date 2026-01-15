using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdToParkedVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "ParkedVehicle",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -8,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -7,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -6,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -5,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -4,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -3,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -2,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ParkedVehicle",
                keyColumn: "Id",
                keyValue: -1,
                column: "OwnerId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_OwnerId",
                table: "ParkedVehicle",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicle_AspNetUsers_OwnerId",
                table: "ParkedVehicle",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicle_AspNetUsers_OwnerId",
                table: "ParkedVehicle");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_OwnerId",
                table: "ParkedVehicle");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ParkedVehicle");
        }
    }
}
