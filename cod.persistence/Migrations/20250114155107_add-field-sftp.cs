using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addfieldsftp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SftpHost",
                schema: "cash",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SftpPast",
                schema: "cash",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SftpPort",
                schema: "cash",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SftpUser",
                schema: "cash",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SftpHost",
                schema: "cash",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "SftpPast",
                schema: "cash",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "SftpPort",
                schema: "cash",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "SftpUser",
                schema: "cash",
                table: "Companies");
        }
    }
}
