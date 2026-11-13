using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingRouteProductCreditPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CreditPrice",
                schema: "data",
                table: "ProductRoutePrices",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditPrice",
                schema: "data",
                table: "ProductRoutePrices");
        }
    }
}
