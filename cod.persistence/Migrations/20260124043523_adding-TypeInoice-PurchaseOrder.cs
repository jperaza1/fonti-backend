using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingTypeInoicePurchaseOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeInoice",
                schema: "operation",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: string.Empty);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeInoice",
                schema: "operation",
                table: "PurchaseOrders");
        }
    }
}
