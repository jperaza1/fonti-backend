using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingInvoiceFieldv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "operation",
                table: "ExpenseInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                schema: "operation",
                table: "ExpenseInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseInvoices_PurchaseOrderId",
                schema: "operation",
                table: "ExpenseInvoices",
                column: "PurchaseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseInvoices_PurchaseOrders_PurchaseOrderId",
                schema: "operation",
                table: "ExpenseInvoices",
                column: "PurchaseOrderId",
                principalSchema: "operation",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseInvoices_PurchaseOrders_PurchaseOrderId",
                schema: "operation",
                table: "ExpenseInvoices");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseInvoices_PurchaseOrderId",
                schema: "operation",
                table: "ExpenseInvoices");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "operation",
                table: "ExpenseInvoices");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                schema: "operation",
                table: "ExpenseInvoices");
        }
    }
}
