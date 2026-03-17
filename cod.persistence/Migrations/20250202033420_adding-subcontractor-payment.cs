using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingsubcontractorpayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Drivers_DriverId",
                schema: "cash",
                table: "Payments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExhibitDate",
                schema: "cash",
                table: "Payments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                schema: "cash",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                schema: "cash",
                table: "Payments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFrom",
                schema: "cash",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTo",
                schema: "cash",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubcontractorId",
                schema: "cash",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SubcontractorId",
                schema: "cash",
                table: "Payments",
                column: "SubcontractorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Drivers_DriverId",
                schema: "cash",
                table: "Payments",
                column: "DriverId",
                principalSchema: "data",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Subcontractors_SubcontractorId",
                schema: "cash",
                table: "Payments",
                column: "SubcontractorId",
                principalSchema: "data",
                principalTable: "Subcontractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Drivers_DriverId",
                schema: "cash",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Subcontractors_SubcontractorId",
                schema: "cash",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_SubcontractorId",
                schema: "cash",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DateFrom",
                schema: "cash",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DateTo",
                schema: "cash",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SubcontractorId",
                schema: "cash",
                table: "Payments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExhibitDate",
                schema: "cash",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                schema: "cash",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                schema: "cash",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Drivers_DriverId",
                schema: "cash",
                table: "Payments",
                column: "DriverId",
                principalSchema: "data",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
