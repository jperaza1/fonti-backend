using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingcafeteria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTenants_Cities_CityId",
                schema: "auth",
                table: "ApplicationTenants");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "data");

            migrationBuilder.DropTable(
                name: "States",
                schema: "data");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "data");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationTenants_CityId",
                schema: "auth",
                table: "ApplicationTenants");

            migrationBuilder.DropColumn(
                name: "CityId",
                schema: "auth",
                table: "ApplicationTenants");

            migrationBuilder.DropColumn(
                name: "Demo",
                schema: "auth",
                table: "ApplicationTenants");

            migrationBuilder.DropColumn(
                name: "DemoTerminationDate",
                schema: "auth",
                table: "ApplicationTenants");

            migrationBuilder.DropColumn(
                name: "FaxNumber",
                schema: "auth",
                table: "ApplicationTenants");

            migrationBuilder.DropColumn(
                name: "Suite",
                schema: "auth",
                table: "ApplicationTenants");

            migrationBuilder.DropColumn(
                name: "WebSite",
                schema: "auth",
                table: "ApplicationTenants");

            migrationBuilder.DropColumn(
                name: "WorkOrderEdiIntegration",
                schema: "auth",
                table: "ApplicationTenants");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                schema: "auth",
                table: "ApplicationTenants");

            migrationBuilder.AddColumn<int>(
                name: "CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cafeterias",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderCorelative = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Cafeterias", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserProfiles_Cafeterias_CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.DropTable(
                name: "Cafeterias",
                schema: "data");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserProfiles_CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.DropColumn(
                name: "CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                schema: "auth",
                table: "ApplicationTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Demo",
                schema: "auth",
                table: "ApplicationTenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DemoTerminationDate",
                schema: "auth",
                table: "ApplicationTenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaxNumber",
                schema: "auth",
                table: "ApplicationTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Suite",
                schema: "auth",
                table: "ApplicationTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebSite",
                schema: "auth",
                table: "ApplicationTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WorkOrderEdiIntegration",
                schema: "auth",
                table: "ApplicationTenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                schema: "auth",
                table: "ApplicationTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "data",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Altitude = table.Column<float>(type: "real", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalSchema: "data",
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTenants_CityId",
                schema: "auth",
                table: "ApplicationTenants",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                schema: "data",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                schema: "data",
                table: "States",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTenants_Cities_CityId",
                schema: "auth",
                table: "ApplicationTenants",
                column: "CityId",
                principalSchema: "data",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
