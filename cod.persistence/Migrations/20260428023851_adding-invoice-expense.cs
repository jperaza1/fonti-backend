using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addinginvoiceexpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Invoices_InvoiceId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                schema: "operation",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DocumentUploadedAt",
                schema: "operation",
                table: "Expenses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentUploadedUser",
                schema: "operation",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExpenseInvoices",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaxableAmount = table.Column<double>(type: "float", nullable: false),
                    ExemptAmount = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    SubTotal = table.Column<double>(type: "float", nullable: false),
                    TaxAmount = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaidUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_ExpenseInvoices", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseInvoices_InvoiceId",
                schema: "operation",
                table: "Expenses",
                column: "InvoiceId",
                principalSchema: "operation",
                principalTable: "ExpenseInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseInvoices_InvoiceId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseInvoices",
                schema: "operation");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "DocumentUploadedAt",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "DocumentUploadedUser",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Invoices_InvoiceId",
                schema: "operation",
                table: "Expenses",
                column: "InvoiceId",
                principalSchema: "operation",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
