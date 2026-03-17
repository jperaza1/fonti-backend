using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingServicePeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServicePeriodId",
                schema: "operation",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ServicePeriodId",
                schema: "operation",
                table: "Sales",
                column: "ServicePeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Types_ServicePeriodId",
                schema: "operation",
                table: "Sales",
                column: "ServicePeriodId",
                principalSchema: "data",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Types_ServicePeriodId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ServicePeriodId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ServicePeriodId",
                schema: "operation",
                table: "Sales");
        }
    }
}
