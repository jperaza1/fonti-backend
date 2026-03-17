using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingDishCafeteria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeAddOnsQuantity",
                schema: "salepoint",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "NoAddOns",
                schema: "salepoint",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "PriceWithCarnet",
                schema: "salepoint",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "PriceWithOutCarnet",
                schema: "salepoint",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "SaleTypeDish",
                schema: "salepoint",
                table: "Dishes");

            migrationBuilder.CreateTable(
                name: "DishesCafeteria",
                schema: "salepoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishId = table.Column<int>(type: "int", nullable: false),
                    CafeteriaId = table.Column<int>(type: "int", nullable: false),
                    SaleTypeDish = table.Column<int>(type: "int", nullable: false),
                    NoAddOns = table.Column<bool>(type: "bit", nullable: false),
                    FreeAddOnsQuantity = table.Column<int>(type: "int", nullable: false),
                    PriceWithCarnet = table.Column<double>(type: "float", nullable: false),
                    PriceWithOutCarnet = table.Column<double>(type: "float", nullable: false),
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
                    table.PrimaryKey("PK_DishesCafeteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishesCafeteria_Cafeterias_CafeteriaId",
                        column: x => x.CafeteriaId,
                        principalSchema: "data",
                        principalTable: "Cafeterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishesCafeteria_Dishes_DishId",
                        column: x => x.DishId,
                        principalSchema: "salepoint",
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishesCafeteria_CafeteriaId",
                schema: "salepoint",
                table: "DishesCafeteria",
                column: "CafeteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_DishesCafeteria_DishId",
                schema: "salepoint",
                table: "DishesCafeteria",
                column: "DishId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishesCafeteria",
                schema: "salepoint");

            migrationBuilder.AddColumn<int>(
                name: "FreeAddOnsQuantity",
                schema: "salepoint",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "NoAddOns",
                schema: "salepoint",
                table: "Dishes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PriceWithCarnet",
                schema: "salepoint",
                table: "Dishes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PriceWithOutCarnet",
                schema: "salepoint",
                table: "Dishes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "SaleTypeDish",
                schema: "salepoint",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
