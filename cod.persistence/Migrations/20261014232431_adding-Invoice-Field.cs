using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingInvoiceField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                schema: "operation",
                table: "ExpenseInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FileUploadedAt",
                schema: "operation",
                table: "ExpenseInvoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUploadedUser",
                schema: "operation",
                table: "ExpenseInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRefund",
                schema: "operation",
                table: "ExpenseInvoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                schema: "operation",
                table: "ExpenseInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                schema: "operation",
                table: "ExpenseInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentId",
                schema: "operation",
                table: "ExpenseInvoices");

            migrationBuilder.DropColumn(
                name: "FileUploadedAt",
                schema: "operation",
                table: "ExpenseInvoices");

            migrationBuilder.DropColumn(
                name: "FileUploadedUser",
                schema: "operation",
                table: "ExpenseInvoices");

            migrationBuilder.DropColumn(
                name: "IsRefund",
                schema: "operation",
                table: "ExpenseInvoices");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                schema: "operation",
                table: "ExpenseInvoices");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                schema: "operation",
                table: "ExpenseInvoices");
        }
    }
}
