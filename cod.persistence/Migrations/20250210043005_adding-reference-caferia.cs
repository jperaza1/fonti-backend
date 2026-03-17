using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingreferencecaferia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                schema: "operation",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reference",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                schema: "operation",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
