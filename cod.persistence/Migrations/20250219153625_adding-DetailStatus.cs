using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingDetailStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CostRecived",
                schema: "operation",
                table: "PurchaseOrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DetailStatus",
                schema: "operation",
                table: "PurchaseOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<double>(
                name: "QuantityRecived",
                schema: "operation",
                table: "PurchaseOrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostRecived",
                schema: "operation",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "DetailStatus",
                schema: "operation",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "QuantityRecived",
                schema: "operation",
                table: "PurchaseOrderDetails");
        }
    }
}
