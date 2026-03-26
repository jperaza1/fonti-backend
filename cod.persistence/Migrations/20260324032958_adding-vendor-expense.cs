using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingvendorexpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Vendors_VendorId",
                schema: "data",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_VendorId",
                schema: "data",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Address",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CreditDays",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "HasCredit",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "HasInvoice",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "RequiresAdminApproval",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Rtn",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Seller",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorId",
                schema: "data",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                schema: "operation",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                schema: "operation",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_InvoiceId",
                schema: "operation",
                table: "Expenses",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_VendorId",
                schema: "operation",
                table: "Expenses",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Invoices_InvoiceId",
                schema: "operation",
                table: "Expenses",
                column: "InvoiceId",
                principalSchema: "operation",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Vendors_VendorId",
                schema: "operation",
                table: "Expenses",
                column: "VendorId",
                principalSchema: "data",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Invoices_InvoiceId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Vendors_VendorId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_InvoiceId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_VendorId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "VendorId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "data",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditDays",
                schema: "data",
                table: "Vendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasCredit",
                schema: "data",
                table: "Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasInvoice",
                schema: "data",
                table: "Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "data",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresAdminApproval",
                schema: "data",
                table: "Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Rtn",
                schema: "data",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seller",
                schema: "data",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                schema: "data",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendorId",
                schema: "data",
                table: "Products",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Vendors_VendorId",
                schema: "data",
                table: "Products",
                column: "VendorId",
                principalSchema: "data",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
