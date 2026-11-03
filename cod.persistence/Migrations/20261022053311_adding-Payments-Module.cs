using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingPaymentsModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccount",
                schema: "data",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountHolder",
                schema: "data",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                schema: "data",
                table: "Vendors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "C1",
                schema: "data",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "C2",
                schema: "data",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "data",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificationHolder",
                schema: "data",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeAccountId",
                schema: "data",
                table: "Vendors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                schema: "operation",
                table: "ExpenseInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankAccountId = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    InvoiceCount = table.Column<double>(type: "float", nullable: false),
                    BankReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalSchema: "finance",
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_PaymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDetails_ExpenseInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "operation",
                        principalTable: "ExpenseInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentDetails_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "finance",
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_BankId",
                schema: "data",
                table: "Vendors",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_TypeAccountId",
                schema: "data",
                table: "Vendors",
                column: "TypeAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseInvoices_PaymentId",
                schema: "operation",
                table: "ExpenseInvoices",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_InvoiceId",
                schema: "finance",
                table: "PaymentDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_PaymentId",
                schema: "finance",
                table: "PaymentDetails",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BankAccountId",
                schema: "finance",
                table: "Payments",
                column: "BankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseInvoices_Payments_PaymentId",
                schema: "operation",
                table: "ExpenseInvoices",
                column: "PaymentId",
                principalSchema: "finance",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Banks_BankId",
                schema: "data",
                table: "Vendors",
                column: "BankId",
                principalSchema: "data",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Types_TypeAccountId",
                schema: "data",
                table: "Vendors",
                column: "TypeAccountId",
                principalSchema: "data",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseInvoices_Payments_PaymentId",
                schema: "operation",
                table: "ExpenseInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Banks_BankId",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Types_TypeAccountId",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropTable(
                name: "PaymentDetails",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "finance");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_BankId",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_TypeAccountId",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseInvoices_PaymentId",
                schema: "operation",
                table: "ExpenseInvoices");

            migrationBuilder.DropColumn(
                name: "BankAccount",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BankAccountHolder",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BankId",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "C1",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "C2",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "IdentificationHolder",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "TypeAccountId",
                schema: "data",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                schema: "operation",
                table: "ExpenseInvoices");
        }
    }
}
