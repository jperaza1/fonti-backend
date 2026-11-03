using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingTruckIdExpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TruckId",
                schema: "operation",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_TruckId",
                schema: "operation",
                table: "Expenses",
                column: "TruckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Trucks_TruckId",
                schema: "operation",
                table: "Expenses",
                column: "TruckId",
                principalSchema: "data",
                principalTable: "Trucks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Trucks_TruckId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_TruckId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "TruckId",
                schema: "operation",
                table: "Expenses");
        }
    }
}
