using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingVendorCafeteria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CafeteriaId",
                schema: "operation",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                schema: "operation",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CafeteriaId",
                schema: "operation",
                table: "Invoices",
                column: "CafeteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_VendorId",
                schema: "operation",
                table: "Invoices",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "Invoices",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Vendors_VendorId",
                schema: "operation",
                table: "Invoices",
                column: "VendorId",
                principalSchema: "data",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Vendors_VendorId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CafeteriaId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_VendorId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CafeteriaId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "VendorId",
                schema: "operation",
                table: "Invoices");
        }
    }
}
