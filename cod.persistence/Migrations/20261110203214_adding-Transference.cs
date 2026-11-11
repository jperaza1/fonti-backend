using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingTransference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CounterpartRouteId",
                schema: "operation",
                table: "DispatchControlDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DispatchTransferId",
                schema: "operation",
                table: "DispatchControlDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DispatchTransfer",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromDispatchControlId = table.Column<int>(type: "int", nullable: false),
                    ToDispatchControlId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DispatchTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispatchTransfer_DispatchControl_FromDispatchControlId",
                        column: x => x.FromDispatchControlId,
                        principalSchema: "operation",
                        principalTable: "DispatchControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispatchTransfer_DispatchControl_ToDispatchControlId",
                        column: x => x.ToDispatchControlId,
                        principalSchema: "operation",
                        principalTable: "DispatchControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispatchControlDetail_CounterpartRouteId",
                schema: "operation",
                table: "DispatchControlDetail",
                column: "CounterpartRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchControlDetail_DispatchTransferId",
                schema: "operation",
                table: "DispatchControlDetail",
                column: "DispatchTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchTransfer_FromDispatchControlId",
                schema: "operation",
                table: "DispatchTransfer",
                column: "FromDispatchControlId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchTransfer_ToDispatchControlId",
                schema: "operation",
                table: "DispatchTransfer",
                column: "ToDispatchControlId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispatchControlDetail_DispatchTransfer_DispatchTransferId",
                schema: "operation",
                table: "DispatchControlDetail",
                column: "DispatchTransferId",
                principalSchema: "operation",
                principalTable: "DispatchTransfer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DispatchControlDetail_Routes_CounterpartRouteId",
                schema: "operation",
                table: "DispatchControlDetail",
                column: "CounterpartRouteId",
                principalSchema: "data",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispatchControlDetail_DispatchTransfer_DispatchTransferId",
                schema: "operation",
                table: "DispatchControlDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DispatchControlDetail_Routes_CounterpartRouteId",
                schema: "operation",
                table: "DispatchControlDetail");

            migrationBuilder.DropTable(
                name: "DispatchTransfer",
                schema: "operation");

            migrationBuilder.DropIndex(
                name: "IX_DispatchControlDetail_CounterpartRouteId",
                schema: "operation",
                table: "DispatchControlDetail");

            migrationBuilder.DropIndex(
                name: "IX_DispatchControlDetail_DispatchTransferId",
                schema: "operation",
                table: "DispatchControlDetail");

            migrationBuilder.DropColumn(
                name: "CounterpartRouteId",
                schema: "operation",
                table: "DispatchControlDetail");

            migrationBuilder.DropColumn(
                name: "DispatchTransferId",
                schema: "operation",
                table: "DispatchControlDetail");
        }
    }
}
