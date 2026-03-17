using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingHasFileInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FileUploadedAt",
                schema: "operation",
                table: "Invoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUploadedUser",
                schema: "operation",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasFile",
                schema: "operation",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileUploadedAt",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "FileUploadedUser",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "HasFile",
                schema: "operation",
                table: "Invoices");
        }
    }
}
