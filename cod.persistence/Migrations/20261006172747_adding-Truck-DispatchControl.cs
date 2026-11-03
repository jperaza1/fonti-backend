using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingTruckDispatchControl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trucks_RouteId",
                schema: "data",
                table: "Trucks");

            migrationBuilder.AddColumn<int>(
                name: "TruckId",
                schema: "operation",
                table: "DispatchControl",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_RouteId",
                schema: "data",
                table: "Trucks",
                column: "RouteId",
                unique: true,
                filter: "[RouteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchControl_TruckId",
                schema: "operation",
                table: "DispatchControl",
                column: "TruckId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispatchControl_Trucks_TruckId",
                schema: "operation",
                table: "DispatchControl",
                column: "TruckId",
                principalSchema: "data",
                principalTable: "Trucks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispatchControl_Trucks_TruckId",
                schema: "operation",
                table: "DispatchControl");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_RouteId",
                schema: "data",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_DispatchControl_TruckId",
                schema: "operation",
                table: "DispatchControl");

            migrationBuilder.DropColumn(
                name: "TruckId",
                schema: "operation",
                table: "DispatchControl");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_RouteId",
                schema: "data",
                table: "Trucks",
                column: "RouteId");
        }
    }
}
