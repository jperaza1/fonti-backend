using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addpercantaje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DiscountPercentage",
                schema: "operation",
                table: "PurchaseOrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxPercentage",
                schema: "operation",
                table: "PurchaseOrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                schema: "operation",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "TaxPercentage",
                schema: "operation",
                table: "PurchaseOrderDetails");
        }
    }
}
