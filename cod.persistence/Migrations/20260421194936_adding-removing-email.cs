using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingremovingemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserProfiles_Cafeterias_CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserProfiles_CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.DropColumn(
                name: "CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "auth",
                table: "ApplicationUserProfiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "auth",
                table: "ApplicationUserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "auth",
                table: "ApplicationUserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserProfiles_CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles",
                column: "CafeteriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserProfiles_Cafeterias_CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
