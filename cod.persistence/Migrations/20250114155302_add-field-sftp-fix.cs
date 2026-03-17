using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addfieldsftpfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SftpPast",
                schema: "cash",
                table: "Companies",
                newName: "SftpPass");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SftpPass",
                schema: "cash",
                table: "Companies",
                newName: "SftpPast");
        }
    }
}
