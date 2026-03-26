using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class updatingsaletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Customers_CustomerId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Registers_RegisterId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Types_CategoryId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Types_ProductTypeId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Types_ServicePeriodId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_CafeteriaId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_CategoryId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_CustomerId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_RegisterId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ServicePeriodId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CafeteriaId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "RegisterId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ServicePeriodId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "ProductTypeId",
                schema: "operation",
                table: "Sales",
                newName: "RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_ProductTypeId",
                schema: "operation",
                table: "Sales",
                newName: "IX_Sales_RouteId");

            migrationBuilder.CreateTable(
                name: "SaleDetails",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "data",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleDetails_Sales_SaleId",
                        column: x => x.SaleId,
                        principalSchema: "operation",
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_ProductId",
                schema: "operation",
                table: "SaleDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_SaleId",
                schema: "operation",
                table: "SaleDetails",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Routes_RouteId",
                schema: "operation",
                table: "Sales",
                column: "RouteId",
                principalSchema: "data",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Routes_RouteId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "SaleDetails",
                schema: "operation");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                schema: "operation",
                table: "Sales",
                newName: "ProductTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_RouteId",
                schema: "operation",
                table: "Sales",
                newName: "IX_Sales_ProductTypeId");

            migrationBuilder.AddColumn<int>(
                name: "CafeteriaId",
                schema: "operation",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "operation",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                schema: "operation",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegisterId",
                schema: "operation",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicePeriodId",
                schema: "operation",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CafeteriaId",
                schema: "operation",
                table: "Sales",
                column: "CafeteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CategoryId",
                schema: "operation",
                table: "Sales",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CustomerId",
                schema: "operation",
                table: "Sales",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_RegisterId",
                schema: "operation",
                table: "Sales",
                column: "RegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ServicePeriodId",
                schema: "operation",
                table: "Sales",
                column: "ServicePeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "Sales",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Customers_CustomerId",
                schema: "operation",
                table: "Sales",
                column: "CustomerId",
                principalSchema: "data",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Registers_RegisterId",
                schema: "operation",
                table: "Sales",
                column: "RegisterId",
                principalSchema: "data",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Types_CategoryId",
                schema: "operation",
                table: "Sales",
                column: "CategoryId",
                principalSchema: "data",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Types_ProductTypeId",
                schema: "operation",
                table: "Sales",
                column: "ProductTypeId",
                principalSchema: "data",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
