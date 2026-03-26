using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingCorrelative : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Correlatives",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Start = table.Column<int>(type: "int", nullable: false),
                    End = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Current = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    EmissionPoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Establishment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Correlatives", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Correlatives",
                schema: "data");
        }
    }
}
