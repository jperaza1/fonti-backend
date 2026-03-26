using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingremovingProductField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Types_MeasureId",
                schema: "data",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Vendors_VendorId",
                schema: "data",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_MeasureId",
                schema: "data",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Cost",
                schema: "data",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Discount",
                schema: "data",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MeasureId",
                schema: "data",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SpecialServiceUse",
                schema: "data",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Tax",
                schema: "data",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitCost",
                schema: "data",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Unit",
                schema: "data",
                table: "Products",
                newName: "ProductType");

            migrationBuilder.AlterColumn<int>(
                name: "VendorId",
                schema: "data",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Vendors_VendorId",
                schema: "data",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductType",
                schema: "data",
                table: "Products",
                newName: "Unit");

            migrationBuilder.AlterColumn<int>(
                name: "VendorId",
                schema: "data",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Cost",
                schema: "data",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                schema: "data",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MeasureId",
                schema: "data",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SpecialServiceUse",
                schema: "data",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Tax",
                schema: "data",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UnitCost",
                schema: "data",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_MeasureId",
                schema: "data",
                table: "Products",
                column: "MeasureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Types_MeasureId",
                schema: "data",
                table: "Products",
                column: "MeasureId",
                principalSchema: "data",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Vendors_VendorId",
                schema: "data",
                table: "Products",
                column: "VendorId",
                principalSchema: "data",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
