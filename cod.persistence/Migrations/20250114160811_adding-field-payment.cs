using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingfieldpayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptPayment",
                schema: "cash",
                table: "Payments");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptedAt",
                schema: "cash",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                schema: "cash",
                table: "Payments",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedAt",
                schema: "cash",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                schema: "cash",
                table: "Payments");

            migrationBuilder.AddColumn<bool>(
                name: "AcceptPayment",
                schema: "cash",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
