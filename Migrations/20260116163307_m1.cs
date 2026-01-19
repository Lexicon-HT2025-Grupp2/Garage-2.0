using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_ParkingSpots_ParkingSpotId",
                table: "Parkings");

            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_Vehicles_VehicleId",
                table: "Parkings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSpots",
                table: "ParkingSpots");

            migrationBuilder.DropColumn(
                name: "ParkingSpots",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "MotorcycleCount",
                table: "ParkingSpots");

            migrationBuilder.DropColumn(
                name: "SpotNumber",
                table: "ParkingSpots");

            migrationBuilder.RenameTable(
                name: "ParkingSpots",
                newName: "ParkingSpot");

            migrationBuilder.RenameColumn(
                name: "ParkedVehicleId",
                table: "ParkingSpot",
                newName: "ParentId");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpotId",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "Parkings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ParkingSpot",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSpot",
                table: "ParkingSpot",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ParkingSpotType",
                columns: table => new
                {
                    ParkingSpotId = table.Column<int>(type: "int", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpotType", x => new { x.ParkingSpotId, x.VehicleTypeId });
                    table.ForeignKey(
                        name: "FK_ParkingSpotType_ParkingSpot_ParkingSpotId",
                        column: x => x.ParkingSpotId,
                        principalTable: "ParkingSpot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingSpotType_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSpotVehicleType",
                columns: table => new
                {
                    PSpotsId = table.Column<int>(type: "int", nullable: false),
                    VehicleTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpotVehicleType", x => new { x.PSpotsId, x.VehicleTypesId });
                    table.ForeignKey(
                        name: "FK_ParkingSpotVehicleType_ParkingSpot_PSpotsId",
                        column: x => x.PSpotsId,
                        principalTable: "ParkingSpot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingSpotVehicleType_VehicleTypes_VehicleTypesId",
                        column: x => x.VehicleTypesId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ParkingSpotId",
                table: "Vehicles",
                column: "ParkingSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpot_ParentId",
                table: "ParkingSpot",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpotType_VehicleTypeId",
                table: "ParkingSpotType",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpotVehicleType_VehicleTypesId",
                table: "ParkingSpotVehicleType",
                column: "VehicleTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_ParkingSpot_ParkingSpotId",
                table: "Parkings",
                column: "ParkingSpotId",
                principalTable: "ParkingSpot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_Vehicles_VehicleId",
                table: "Parkings",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpot_ParkingSpot_ParentId",
                table: "ParkingSpot",
                column: "ParentId",
                principalTable: "ParkingSpot",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_ParkingSpot_ParkingSpotId",
                table: "Vehicles",
                column: "ParkingSpotId",
                principalTable: "ParkingSpot",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_ParkingSpot_ParkingSpotId",
                table: "Parkings");

            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_Vehicles_VehicleId",
                table: "Parkings");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpot_ParkingSpot_ParentId",
                table: "ParkingSpot");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_ParkingSpot_ParkingSpotId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "ParkingSpotType");

            migrationBuilder.DropTable(
                name: "ParkingSpotVehicleType");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_ParkingSpotId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSpot",
                table: "ParkingSpot");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSpot_ParentId",
                table: "ParkingSpot");

            migrationBuilder.DropColumn(
                name: "ParkingSpotId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ParkingSpot");

            migrationBuilder.RenameTable(
                name: "ParkingSpot",
                newName: "ParkingSpots");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "ParkingSpots",
                newName: "ParkedVehicleId");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParkingSpots",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "Parkings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MotorcycleCount",
                table: "ParkingSpots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpotNumber",
                table: "ParkingSpots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSpots",
                table: "ParkingSpots",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_ParkingSpots_ParkingSpotId",
                table: "Parkings",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_Vehicles_VehicleId",
                table: "Parkings",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
