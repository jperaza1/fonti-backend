using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingnewfieldemployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHired",
                schema: "rrhh",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "EmploymentStatus",
                schema: "rrhh",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shift",
                schema: "rrhh",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmploymentStatus",
                schema: "rrhh",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Shift",
                schema: "rrhh",
                table: "Employees");

            migrationBuilder.AddColumn<bool>(
                name: "IsHired",
                schema: "rrhh",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
