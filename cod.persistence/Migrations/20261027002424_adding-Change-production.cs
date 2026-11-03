using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingChangeproduction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductionShiftId",
                schema: "operation",
                table: "ProductionOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductionShifts",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ProductionShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionShifts_Types_ShiftTypeId",
                        column: x => x.ShiftTypeId,
                        principalSchema: "data",
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_ProductionShiftId",
                schema: "operation",
                table: "ProductionOrders",
                column: "ProductionShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionShifts_ProductionDate_ShiftTypeId",
                schema: "operation",
                table: "ProductionShifts",
                columns: new[] { "ProductionDate", "ShiftTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionShifts_ShiftTypeId",
                schema: "operation",
                table: "ProductionShifts",
                column: "ShiftTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_ProductionShifts_ProductionShiftId",
                schema: "operation",
                table: "ProductionOrders",
                column: "ProductionShiftId",
                principalSchema: "operation",
                principalTable: "ProductionShifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_ProductionShifts_ProductionShiftId",
                schema: "operation",
                table: "ProductionOrders");

            migrationBuilder.DropTable(
                name: "ProductionShifts",
                schema: "operation");

            migrationBuilder.DropIndex(
                name: "IX_ProductionOrders_ProductionShiftId",
                schema: "operation",
                table: "ProductionOrders");

            migrationBuilder.DropColumn(
                name: "ProductionShiftId",
                schema: "operation",
                table: "ProductionOrders");
        }
    }
}
