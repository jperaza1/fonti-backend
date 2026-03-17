using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingRegisterininventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseInInventory",
                schema: "data",
                table: "Registers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RegisterId",
                schema: "operation",
                table: "Inventories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_RegisterId",
                schema: "operation",
                table: "Inventories",
                column: "RegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Registers_RegisterId",
                schema: "operation",
                table: "Inventories",
                column: "RegisterId",
                principalSchema: "data",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Registers_RegisterId",
                schema: "operation",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_RegisterId",
                schema: "operation",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "UseInInventory",
                schema: "data",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "RegisterId",
                schema: "operation",
                table: "Inventories");
        }
    }
}
