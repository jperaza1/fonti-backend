using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingSaleCreditFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ShowCreditPrice",
                schema: "data",
                table: "ProductRoutePrices",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ShowCreditPrice",
                schema: "data",
                table: "ProductRoutePrices",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
