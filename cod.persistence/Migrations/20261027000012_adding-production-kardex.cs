using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingproductionkardex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductionOrderId",
                schema: "operation",
                table: "InventoryKardex",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductionOrders",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftTypeId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_ProductionOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "data",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_Types_ShiftTypeId",
                        column: x => x.ShiftTypeId,
                        principalSchema: "data",
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryKardex_ProductionOrderId",
                schema: "operation",
                table: "InventoryKardex",
                column: "ProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_ProductId",
                schema: "operation",
                table: "ProductionOrders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_ShiftTypeId",
                schema: "operation",
                table: "ProductionOrders",
                column: "ShiftTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryKardex_ProductionOrders_ProductionOrderId",
                schema: "operation",
                table: "InventoryKardex",
                column: "ProductionOrderId",
                principalSchema: "operation",
                principalTable: "ProductionOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryKardex_ProductionOrders_ProductionOrderId",
                schema: "operation",
                table: "InventoryKardex");

            migrationBuilder.DropTable(
                name: "ProductionOrders",
                schema: "operation");

            migrationBuilder.DropIndex(
                name: "IX_InventoryKardex_ProductionOrderId",
                schema: "operation",
                table: "InventoryKardex");

            migrationBuilder.DropColumn(
                name: "ProductionOrderId",
                schema: "operation",
                table: "InventoryKardex");
        }
    }
}
