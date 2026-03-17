using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingSideItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Cafeterias_CafeteriaId",
                schema: "salepoint",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_CafeteriaId",
                schema: "salepoint",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CafeteriaId",
                schema: "salepoint",
                table: "Dishes");

            migrationBuilder.CreateTable(
                name: "SideItems",
                schema: "salepoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_SideItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SideItems",
                schema: "salepoint");

            migrationBuilder.AddColumn<int>(
                name: "CafeteriaId",
                schema: "salepoint",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CafeteriaId",
                schema: "salepoint",
                table: "Dishes",
                column: "CafeteriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Cafeterias_CafeteriaId",
                schema: "salepoint",
                table: "Dishes",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
