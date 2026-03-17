using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "operation",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CategoryId",
                schema: "operation",
                table: "Invoices",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Types_CategoryId",
                schema: "operation",
                table: "Invoices",
                column: "CategoryId",
                principalSchema: "data",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Types_CategoryId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CategoryId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "operation",
                table: "Invoices");
        }
    }
}
