using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingwarehouses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductStock_ProductId",
                schema: "operation",
                table: "ProductStock");

            migrationBuilder.CreateTable(
                name: "Warehouses",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kind = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.Sql(@"
INSERT INTO operation.Warehouses (Name, Kind, IsActive, CreationTime, CreatorUser)
VALUES (N'Principal', 1, 1, SYSUTCDATETIME(), N'migracion'),
       (N'Producción', 2, 1, SYSUTCDATETIME(), N'migracion'),
       (N'Terminado', 3, 1, SYSUTCDATETIME(), N'migracion');
");

            migrationBuilder.Sql(@"
ALTER TABLE operation.ProductStock ADD WarehouseId int NULL;
ALTER TABLE operation.InventoryKardex ADD WarehouseId int NULL;
");

            migrationBuilder.Sql(@"
UPDATE stock
SET WarehouseId = CASE WHEN product.ProductType = 1 THEN finished.Id ELSE principal.Id END
FROM operation.ProductStock stock
LEFT JOIN data.Products product ON product.Id = stock.ProductId
JOIN operation.Warehouses principal ON principal.Kind = 1
JOIN operation.Warehouses finished ON finished.Kind = 3;

UPDATE kardex
SET WarehouseId = CASE WHEN product.ProductType = 1 THEN finished.Id ELSE principal.Id END
FROM operation.InventoryKardex kardex
LEFT JOIN data.Products product ON product.Id = kardex.ProductId
JOIN operation.Warehouses principal ON principal.Kind = 1
JOIN operation.Warehouses finished ON finished.Kind = 3;
");

            migrationBuilder.Sql(@"
ALTER TABLE operation.ProductStock ALTER COLUMN WarehouseId int NOT NULL;
ALTER TABLE operation.InventoryKardex ALTER COLUMN WarehouseId int NOT NULL;
");

            migrationBuilder.CreateTable(
                name: "WarehouseTransfers",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromWarehouseId = table.Column<int>(type: "int", nullable: false),
                    ToWarehouseId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SentUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcceptedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_WarehouseTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseTransfers_Warehouses_FromWarehouseId",
                        column: x => x.FromWarehouseId,
                        principalSchema: "operation",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTransfers_Warehouses_ToWarehouseId",
                        column: x => x.ToWarehouseId,
                        principalSchema: "operation",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseTransferDetails",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseTransferId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                    table.PrimaryKey("PK_WarehouseTransferDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseTransferDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "data",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseTransferDetails_WarehouseTransfers_WarehouseTransferId",
                        column: x => x.WarehouseTransferId,
                        principalSchema: "operation",
                        principalTable: "WarehouseTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStock_ProductId_WarehouseId",
                schema: "operation",
                table: "ProductStock",
                columns: new[] { "ProductId", "WarehouseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductStock_WarehouseId",
                schema: "operation",
                table: "ProductStock",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryKardex_WarehouseId",
                schema: "operation",
                table: "InventoryKardex",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Kind",
                schema: "operation",
                table: "Warehouses",
                column: "Kind",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransferDetails_ProductId",
                schema: "operation",
                table: "WarehouseTransferDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransferDetails_WarehouseTransferId",
                schema: "operation",
                table: "WarehouseTransferDetails",
                column: "WarehouseTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransfers_FromWarehouseId",
                schema: "operation",
                table: "WarehouseTransfers",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransfers_ToWarehouseId",
                schema: "operation",
                table: "WarehouseTransfers",
                column: "ToWarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryKardex_Warehouses_WarehouseId",
                schema: "operation",
                table: "InventoryKardex",
                column: "WarehouseId",
                principalSchema: "operation",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_Warehouses_WarehouseId",
                schema: "operation",
                table: "ProductStock",
                column: "WarehouseId",
                principalSchema: "operation",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryKardex_Warehouses_WarehouseId",
                schema: "operation",
                table: "InventoryKardex");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_Warehouses_WarehouseId",
                schema: "operation",
                table: "ProductStock");

            migrationBuilder.DropTable(
                name: "WarehouseTransferDetails",
                schema: "operation");

            migrationBuilder.DropTable(
                name: "WarehouseTransfers",
                schema: "operation");

            migrationBuilder.DropTable(
                name: "Warehouses",
                schema: "operation");

            migrationBuilder.DropIndex(
                name: "IX_ProductStock_ProductId_WarehouseId",
                schema: "operation",
                table: "ProductStock");

            migrationBuilder.DropIndex(
                name: "IX_ProductStock_WarehouseId",
                schema: "operation",
                table: "ProductStock");

            migrationBuilder.DropIndex(
                name: "IX_InventoryKardex_WarehouseId",
                schema: "operation",
                table: "InventoryKardex");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                schema: "operation",
                table: "ProductStock");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                schema: "operation",
                table: "InventoryKardex");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStock_ProductId",
                schema: "operation",
                table: "ProductStock",
                column: "ProductId",
                unique: true);
        }
    }
}
