using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingCanceled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CanceledAt",
                schema: "operation",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CanceledUser",
                schema: "operation",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonCanceled",
                schema: "operation",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanceledAt",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CanceledUser",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ReasonCanceled",
                schema: "operation",
                table: "PurchaseOrders");
        }
    }
}
