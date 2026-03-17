using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingHasCreditVendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<double>(
                name: "TaxAmount",
                schema: "operation",
                table: "PurchaseOrderInvoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditDays",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "HasCredit",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                schema: "operation",
                table: "PurchaseOrderInvoices");
        }
    }
}
