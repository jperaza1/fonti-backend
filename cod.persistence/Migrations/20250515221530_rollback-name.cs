using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class rollbackname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceDetails_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServices_Products_ProductId",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServices_SpecialServiceDetails_SpecialServiceId",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropIndex(
                name: "IX_SpecialServices_SpecialServiceId",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.DropColumn(
                name: "ApprovedUser",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.DropColumn(
                name: "Date",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.DropColumn(
                name: "ReasonRejection",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.DropColumn(
                name: "Reference",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.DropColumn(
                name: "RejectedAt",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.DropColumn(
                name: "RejectedUser",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.RenameColumn(
                name: "SpecialServiceId",
                schema: "operation",
                table: "SpecialServices",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                schema: "operation",
                table: "SpecialServices",
                newName: "CafeteriaId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialServices_ProductId",
                schema: "operation",
                table: "SpecialServices",
                newName: "IX_SpecialServices_CafeteriaId");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "operation",
                table: "SpecialServiceDetails",
                newName: "SpecialServiceId");

            migrationBuilder.RenameColumn(
                name: "CafeteriaId",
                schema: "operation",
                table: "SpecialServiceDetails",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialServiceDetails_CafeteriaId",
                schema: "operation",
                table: "SpecialServiceDetails",
                newName: "IX_SpecialServiceDetails_ProductId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                schema: "operation",
                table: "SpecialServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedUser",
                schema: "operation",
                table: "SpecialServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                schema: "operation",
                table: "SpecialServices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReasonRejection",
                schema: "operation",
                table: "SpecialServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                schema: "operation",
                table: "SpecialServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                schema: "operation",
                table: "SpecialServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectedUser",
                schema: "operation",
                table: "SpecialServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                schema: "operation",
                table: "SpecialServiceDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialServiceDetails_SpecialServiceId",
                schema: "operation",
                table: "SpecialServiceDetails",
                column: "SpecialServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceDetails_Products_ProductId",
                schema: "operation",
                table: "SpecialServiceDetails",
                column: "ProductId",
                principalSchema: "data",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceDetails_SpecialServices_SpecialServiceId",
                schema: "operation",
                table: "SpecialServiceDetails",
                column: "SpecialServiceId",
                principalSchema: "operation",
                principalTable: "SpecialServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServices_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "SpecialServices",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceDetails_Products_ProductId",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceDetails_SpecialServices_SpecialServiceId",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServices_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropIndex(
                name: "IX_SpecialServiceDetails_SpecialServiceId",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropColumn(
                name: "ApprovedUser",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropColumn(
                name: "Date",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropColumn(
                name: "ReasonRejection",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropColumn(
                name: "Reference",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropColumn(
                name: "RejectedAt",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropColumn(
                name: "RejectedUser",
                schema: "operation",
                table: "SpecialServices");

            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "operation",
                table: "SpecialServiceDetails");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "operation",
                table: "SpecialServices",
                newName: "SpecialServiceId");

            migrationBuilder.RenameColumn(
                name: "CafeteriaId",
                schema: "operation",
                table: "SpecialServices",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialServices_CafeteriaId",
                schema: "operation",
                table: "SpecialServices",
                newName: "IX_SpecialServices_ProductId");

            migrationBuilder.RenameColumn(
                name: "SpecialServiceId",
                schema: "operation",
                table: "SpecialServiceDetails",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                schema: "operation",
                table: "SpecialServiceDetails",
                newName: "CafeteriaId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialServiceDetails_ProductId",
                schema: "operation",
                table: "SpecialServiceDetails",
                newName: "IX_SpecialServiceDetails_CafeteriaId");

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                schema: "operation",
                table: "SpecialServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                schema: "operation",
                table: "SpecialServiceDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedUser",
                schema: "operation",
                table: "SpecialServiceDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                schema: "operation",
                table: "SpecialServiceDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReasonRejection",
                schema: "operation",
                table: "SpecialServiceDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                schema: "operation",
                table: "SpecialServiceDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                schema: "operation",
                table: "SpecialServiceDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectedUser",
                schema: "operation",
                table: "SpecialServiceDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialServices_SpecialServiceId",
                schema: "operation",
                table: "SpecialServices",
                column: "SpecialServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceDetails_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "SpecialServiceDetails",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServices_Products_ProductId",
                schema: "operation",
                table: "SpecialServices",
                column: "ProductId",
                principalSchema: "data",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServices_SpecialServiceDetails_SpecialServiceId",
                schema: "operation",
                table: "SpecialServices",
                column: "SpecialServiceId",
                principalSchema: "operation",
                principalTable: "SpecialServiceDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
