using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class fixDispatchControlDetailId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMovement_DispatchControlDetail_DispatchControlId",
                schema: "operation",
                table: "ProductMovement");

            migrationBuilder.DropIndex(
                name: "IX_ProductMovement_DispatchControlId",
                schema: "operation",
                table: "ProductMovement");

            migrationBuilder.DropColumn(
                name: "DispatchControlId",
                schema: "operation",
                table: "ProductMovement");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMovement_DispatchControlDetailId",
                schema: "operation",
                table: "ProductMovement",
                column: "DispatchControlDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMovement_DispatchControlDetail_DispatchControlDetailId",
                schema: "operation",
                table: "ProductMovement",
                column: "DispatchControlDetailId",
                principalSchema: "operation",
                principalTable: "DispatchControlDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMovement_DispatchControlDetail_DispatchControlDetailId",
                schema: "operation",
                table: "ProductMovement");

            migrationBuilder.DropIndex(
                name: "IX_ProductMovement_DispatchControlDetailId",
                schema: "operation",
                table: "ProductMovement");

            migrationBuilder.AddColumn<int>(
                name: "DispatchControlId",
                schema: "operation",
                table: "ProductMovement",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMovement_DispatchControlId",
                schema: "operation",
                table: "ProductMovement",
                column: "DispatchControlId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMovement_DispatchControlDetail_DispatchControlId",
                schema: "operation",
                table: "ProductMovement",
                column: "DispatchControlId",
                principalSchema: "operation",
                principalTable: "DispatchControlDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
