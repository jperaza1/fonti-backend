using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Payments_PaymentId",
                schema: "cash",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentId",
                schema: "cash",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                schema: "cash",
                table: "Payments");

            migrationBuilder.AddColumn<bool>(
                name: "Seen",
                schema: "cash",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SeenAt",
                schema: "cash",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ServiceFee",
                schema: "cash",
                table: "Payments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "cash",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Deductions",
                schema: "cash",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Deductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deductions_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "cash",
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_PaymentId",
                schema: "cash",
                table: "Deductions",
                column: "PaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deductions",
                schema: "cash");

            migrationBuilder.DropColumn(
                name: "Seen",
                schema: "cash",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SeenAt",
                schema: "cash",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ServiceFee",
                schema: "cash",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "cash",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                schema: "cash",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentId",
                schema: "cash",
                table: "Payments",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Payments_PaymentId",
                schema: "cash",
                table: "Payments",
                column: "PaymentId",
                principalSchema: "cash",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
