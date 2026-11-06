using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using cod.persistence.Implementations;

namespace cod.persistence.Migrations
{
    [DbContext(typeof(Repository))]
    [Migration("20261106032600_adding-vendor-route-sale")]
    public partial class addingvendorroutesale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('data.Routes', 'Vendor') IS NULL
    ALTER TABLE [data].[Routes] ADD [Vendor] nvarchar(max) NULL;

IF COL_LENGTH('operation.Sales', 'Vendor') IS NULL
    ALTER TABLE [operation].[Sales] ADD [Vendor] nvarchar(max) NULL;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('operation.Sales', 'Vendor') IS NOT NULL
    ALTER TABLE [operation].[Sales] DROP COLUMN [Vendor];

IF COL_LENGTH('data.Routes', 'Vendor') IS NOT NULL
    ALTER TABLE [data].[Routes] DROP COLUMN [Vendor];
");
        }
    }
}
