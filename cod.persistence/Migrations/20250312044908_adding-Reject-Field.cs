using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingRejectField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReasonRejection",
                schema: "operation",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                schema: "operation",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectedUser",
                schema: "operation",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasonRejection",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "RejectedAt",
                schema: "operation",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "RejectedUser",
                schema: "operation",
                table: "PurchaseOrders");
        }
    }
}
