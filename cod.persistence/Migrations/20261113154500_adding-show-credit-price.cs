using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using cod.persistence.Implementations;

namespace cod.persistence.Migrations
{
    [DbContext(typeof(Repository))]
    [Migration("20261113154500_adding-show-credit-price")]
    public partial class addingshowcreditprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('data.ProductRoutePrices', 'ShowCreditPrice') IS NULL
    ALTER TABLE [data].[ProductRoutePrices] ADD [ShowCreditPrice] bit NOT NULL CONSTRAINT [DF_ProductRoutePrices_ShowCreditPrice] DEFAULT 0;

UPDATE [data].[ProductRoutePrices]
SET [ShowCreditPrice] = 1
WHERE [CreditPrice] IS NOT NULL AND [CreditPrice] > 0;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('data.ProductRoutePrices', 'ShowCreditPrice') IS NOT NULL
BEGIN
    ALTER TABLE [data].[ProductRoutePrices] DROP CONSTRAINT [DF_ProductRoutePrices_ShowCreditPrice];
    ALTER TABLE [data].[ProductRoutePrices] DROP COLUMN [ShowCreditPrice];
END
");
        }
    }
}
