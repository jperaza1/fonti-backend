using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingordenescomprafonti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_Products_ProductId",
                schema: "operation",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_CafeteriaId",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CafeteriaId",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                schema: "operation",
                table: "PurchaseOrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductVendorId",
                schema: "operation",
                table: "PurchaseOrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PriceApproval",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatePriceApproval = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_PriceApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceApproval_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "operation",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceApprovalDetails",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceApprovalId = table.Column<int>(type: "int", nullable: false),
                    ProductVendorId = table.Column<int>(type: "int", nullable: false),
                    OldCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    NewCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
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
                    table.PrimaryKey("PK_PriceApprovalDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceApprovalDetails_PriceApproval_PriceApprovalId",
                        column: x => x.PriceApprovalId,
                        principalSchema: "operation",
                        principalTable: "PriceApproval",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceApprovalDetails_ProductVendors_ProductVendorId",
                        column: x => x.ProductVendorId,
                        principalSchema: "data",
                        principalTable: "ProductVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_ProductVendorId",
                schema: "operation",
                table: "PurchaseOrderDetails",
                column: "ProductVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceApproval_PurchaseOrderId",
                schema: "operation",
                table: "PriceApproval",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceApprovalDetails_PriceApprovalId",
                schema: "operation",
                table: "PriceApprovalDetails",
                column: "PriceApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceApprovalDetails_ProductVendorId",
                schema: "operation",
                table: "PriceApprovalDetails",
                column: "ProductVendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_Products_ProductId",
                schema: "operation",
                table: "PurchaseOrderDetails",
                column: "ProductId",
                principalSchema: "data",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_ProductVendors_ProductVendorId",
                schema: "operation",
                table: "PurchaseOrderDetails",
                column: "ProductVendorId",
                principalSchema: "data",
                principalTable: "ProductVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_Products_ProductId",
                schema: "operation",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_ProductVendors_ProductVendorId",
                schema: "operation",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "PriceApprovalDetails",
                schema: "operation");

            migrationBuilder.DropTable(
                name: "PriceApproval",
                schema: "operation");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderDetails_ProductVendorId",
                schema: "operation",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductVendorId",
                schema: "operation",
                table: "PurchaseOrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "CafeteriaId",
                schema: "operation",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                schema: "operation",
                table: "PurchaseOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CafeteriaId",
                schema: "operation",
                table: "PurchaseOrders",
                column: "CafeteriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_Products_ProductId",
                schema: "operation",
                table: "PurchaseOrderDetails",
                column: "ProductId",
                principalSchema: "data",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "PurchaseOrders",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
