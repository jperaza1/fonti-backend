using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingProductRoutePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Routes",
                schema: "operation",
                newName: "Routes",
                newSchema: "data");

            migrationBuilder.CreateTable(
                name: "ProductRoutePrices",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
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
                    table.PrimaryKey("PK_ProductRoutePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRoutePrices_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "data",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRoutePrices_Routes_RouteId",
                        column: x => x.RouteId,
                        principalSchema: "data",
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRoutePrices_ProductId",
                schema: "data",
                table: "ProductRoutePrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRoutePrices_RouteId",
                schema: "data",
                table: "ProductRoutePrices",
                column: "RouteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRoutePrices",
                schema: "data");

            migrationBuilder.RenameTable(
                name: "Routes",
                schema: "data",
                newName: "Routes",
                newSchema: "operation");
        }
    }
}
