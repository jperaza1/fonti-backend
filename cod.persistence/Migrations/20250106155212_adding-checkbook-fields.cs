using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingcheckbookfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ZellerUserName",
                schema: "data",
                table: "Drivers",
                newName: "ZelleUserName");

            migrationBuilder.AddColumn<string>(
                name: "CheckBookId",
                schema: "data",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckBookKey",
                schema: "data",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckBookSecret",
                schema: "data",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "data",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckBookId",
                schema: "data",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "CheckBookKey",
                schema: "data",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "CheckBookSecret",
                schema: "data",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "data",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "ZelleUserName",
                schema: "data",
                table: "Drivers",
                newName: "ZellerUserName");
        }
    }
}
