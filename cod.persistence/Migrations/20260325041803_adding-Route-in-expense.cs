using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingRouteinexpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                schema: "operation",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_RouteId",
                schema: "operation",
                table: "Expenses",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Routes_RouteId",
                schema: "operation",
                table: "Expenses",
                column: "RouteId",
                principalSchema: "data",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Routes_RouteId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_RouteId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "RouteId",
                schema: "operation",
                table: "Expenses");
        }
    }
}
