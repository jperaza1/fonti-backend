using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingnewfieldv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Cafeterias_CafeteriaId",
                schema: "rrhh",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Types_PositionId",
                schema: "rrhh",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Types_UniformSizeId",
                schema: "rrhh",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "UniformSizeId",
                schema: "rrhh",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                schema: "rrhh",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CafeteriaId",
                schema: "rrhh",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Cafeterias_CafeteriaId",
                schema: "rrhh",
                table: "Employees",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Types_PositionId",
                schema: "rrhh",
                table: "Employees",
                column: "PositionId",
                principalSchema: "data",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Types_UniformSizeId",
                schema: "rrhh",
                table: "Employees",
                column: "UniformSizeId",
                principalSchema: "data",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Cafeterias_CafeteriaId",
                schema: "rrhh",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Types_PositionId",
                schema: "rrhh",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Types_UniformSizeId",
                schema: "rrhh",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "UniformSizeId",
                schema: "rrhh",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                schema: "rrhh",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CafeteriaId",
                schema: "rrhh",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Cafeterias_CafeteriaId",
                schema: "rrhh",
                table: "Employees",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Types_PositionId",
                schema: "rrhh",
                table: "Employees",
                column: "PositionId",
                principalSchema: "data",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Types_UniformSizeId",
                schema: "rrhh",
                table: "Employees",
                column: "UniformSizeId",
                principalSchema: "data",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
