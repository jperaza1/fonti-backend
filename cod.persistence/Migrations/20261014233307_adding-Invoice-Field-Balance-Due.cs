using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingInvoiceFieldBalanceDue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Balance",
                schema: "operation",
                table: "ExpenseInvoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                schema: "operation",
                table: "ExpenseInvoices",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                schema: "operation",
                table: "ExpenseInvoices");

            migrationBuilder.DropColumn(
                name: "DueDate",
                schema: "operation",
                table: "ExpenseInvoices");
        }
    }
}
