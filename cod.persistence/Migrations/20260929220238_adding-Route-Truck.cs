using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingRouteTruck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                schema: "data",
                table: "Trucks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_RouteId",
                schema: "data",
                table: "Trucks",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_Routes_RouteId",
                schema: "data",
                table: "Trucks",
                column: "RouteId",
                principalSchema: "data",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_Routes_RouteId",
                schema: "data",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_RouteId",
                schema: "data",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "RouteId",
                schema: "data",
                table: "Trucks");
        }
    }
}
