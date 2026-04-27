using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class addingInvoiceRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InvoiceRequired",
                schema: "data",
                table: "SubCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceRequired",
                schema: "data",
                table: "SubCategories");
        }
    }
}
