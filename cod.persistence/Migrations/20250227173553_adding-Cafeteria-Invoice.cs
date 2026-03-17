using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingCafeteriaInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.AlterColumn<int>(
                name: "CafeteriaId",
                schema: "operation",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "Invoices",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.AlterColumn<int>(
                name: "CafeteriaId",
                schema: "operation",
                table: "Invoices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "Invoices",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
