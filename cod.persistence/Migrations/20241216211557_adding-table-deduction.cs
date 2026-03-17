using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingtablededuction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayBasis",
                schema: "cash",
                table: "Payments",
                newName: "LoadAmount");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "LoadAmount",
                schema: "cash",
                table: "Payments",
                newName: "PayBasis");
        }
    }
}
