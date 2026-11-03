using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingInvoiceSales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "finance");

            migrationBuilder.AddColumn<int>(
                name: "SaleInvoiceId",
                schema: "operation",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Current",
                schema: "data",
                table: "Correlatives",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");

            migrationBuilder.CreateTable(
                name: "saleInvoices",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CorrelativeId = table.Column<int>(type: "int", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaxableAmount = table.Column<double>(type: "float", nullable: false),
                    TaxableAmount18 = table.Column<double>(type: "float", nullable: false),
                    ExemptAmount = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    SubTotal = table.Column<double>(type: "float", nullable: false),
                    TaxAmount = table.Column<double>(type: "float", nullable: false),
                    Tax18Amount = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaidUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_saleInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_saleInvoices_Correlatives_CorrelativeId",
                        column: x => x.CorrelativeId,
                        principalSchema: "data",
                        principalTable: "Correlatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_saleInvoices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "data",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "saleInvoiceDetails",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleInvoiceId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_saleInvoiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_saleInvoiceDetails_saleInvoices_SaleInvoiceId",
                        column: x => x.SaleInvoiceId,
                        principalSchema: "finance",
                        principalTable: "saleInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_saleInvoiceDetails_Sales_SaleId",
                        column: x => x.SaleId,
                        principalSchema: "operation",
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SaleInvoiceId",
                schema: "operation",
                table: "Sales",
                column: "SaleInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_saleInvoiceDetails_SaleId",
                schema: "finance",
                table: "saleInvoiceDetails",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_saleInvoiceDetails_SaleInvoiceId",
                schema: "finance",
                table: "saleInvoiceDetails",
                column: "SaleInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_saleInvoices_CorrelativeId",
                schema: "finance",
                table: "saleInvoices",
                column: "CorrelativeId");

            migrationBuilder.CreateIndex(
                name: "IX_saleInvoices_CustomerId",
                schema: "finance",
                table: "saleInvoices",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_saleInvoices_SaleInvoiceId",
                schema: "operation",
                table: "Sales",
                column: "SaleInvoiceId",
                principalSchema: "finance",
                principalTable: "saleInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_saleInvoices_SaleInvoiceId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "saleInvoiceDetails",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "saleInvoices",
                schema: "finance");

            migrationBuilder.DropIndex(
                name: "IX_Sales_SaleInvoiceId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SaleInvoiceId",
                schema: "operation",
                table: "Sales");

            migrationBuilder.AlterColumn<decimal>(
                name: "Current",
                schema: "data",
                table: "Correlatives",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
