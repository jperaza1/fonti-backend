using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingSubTotalRecived : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserProfiles_UserId",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.AddColumn<double>(
                name: "DiscountRecived",
                schema: "operation",
                table: "PurchaseOrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SubTotalRecived",
                schema: "operation",
                table: "PurchaseOrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxRecived",
                schema: "operation",
                table: "PurchaseOrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserProfiles_UserId",
                schema: "auth",
                table: "ApplicationUserProfiles",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserProfiles_UserId",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.DropColumn(
                name: "DiscountRecived",
                schema: "operation",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "SubTotalRecived",
                schema: "operation",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "TaxRecived",
                schema: "operation",
                table: "PurchaseOrderDetails");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserProfiles_UserId",
                schema: "auth",
                table: "ApplicationUserProfiles",
                column: "UserId");
        }
    }
}
