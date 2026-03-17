using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingpurchaseorderstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                schema: "operation",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedUser",
                schema: "operation",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                schema: "operation",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompletedUser",
                schema: "operation",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                schema: "operation",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessingAt",
                schema: "operation",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessingUser",
                schema: "operation",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ApprovedUser",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CompletedUser",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ProcessingAt",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ProcessingUser",
                schema: "operation",
                table: "PurchaseOrders");
        }
    }
}
