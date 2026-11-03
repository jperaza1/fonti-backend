using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using cod.persistence.Implementations;

namespace cod.persistence.Migrations
{
    [DbContext(typeof(Repository))]
    [Migration("20261023222800_adding-Distributor")]
    public partial class addingDistributor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Distributors",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Distributors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductDistributorPrices",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistributorId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
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
                    table.PrimaryKey("PK_ProductDistributorPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDistributorPrices_Distributors_DistributorId",
                        column: x => x.DistributorId,
                        principalSchema: "data",
                        principalTable: "Distributors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDistributorPrices_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "data",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDistributorPrices_DistributorId",
                schema: "data",
                table: "ProductDistributorPrices",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDistributorPrices_ProductId",
                schema: "data",
                table: "ProductDistributorPrices",
                column: "ProductId");

            migrationBuilder.Sql(@"
DECLARE @now datetime2 = GETUTCDATE();

IF OBJECT_ID('tempdb..#DistributorPermissions') IS NOT NULL
    DROP TABLE #DistributorPermissions;

CREATE TABLE #DistributorPermissions
(
    Name NVARCHAR(256) NOT NULL,
    NormalizedName NVARCHAR(256) NOT NULL,
    Id NVARCHAR(450) NOT NULL
);

INSERT INTO #DistributorPermissions (Name, NormalizedName, Id)
VALUES
    ('DATA.DISTRIBUTOR', 'DATA.DISTRIBUTOR', NEWID()),
    ('DATA.DISTRIBUTOR.CREATE', 'DATA.DISTRIBUTOR.CREATE', NEWID()),
    ('DATA.DISTRIBUTOR.UPDATE', 'DATA.DISTRIBUTOR.UPDATE', NEWID()),
    ('DATA.DISTRIBUTOR.DISABLE', 'DATA.DISTRIBUTOR.DISABLE', NEWID()),
    ('DATA.DISTRIBUTOR.ENABLE', 'DATA.DISTRIBUTOR.ENABLE', NEWID());

INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
SELECT p.Id, p.Name, p.NormalizedName, NEWID()
FROM #DistributorPermissions p
WHERE NOT EXISTS (SELECT 1 FROM AspNetRoles r WHERE r.Name = p.Name);

UPDATE p
SET p.Id = r.Id
FROM #DistributorPermissions p
INNER JOIN AspNetRoles r ON r.Name = p.Name;

INSERT INTO auth.ApplicationGroupRoles (Name, GroupHeaderId, IsActive, CreationTime, CreatorUser)
SELECT DISTINCT 'Distribuidores', gr.GroupHeaderId, 1, @now, 'MIGRATION'
FROM auth.ApplicationGroupRoles gr
INNER JOIN auth.ApplicationGroupRolePermissions grp ON grp.GroupRoleId = gr.Id
INNER JOIN AspNetRoles r ON r.Id = grp.PermissionId
WHERE r.Name = 'DATA.ROUTE'
  AND NOT EXISTS (
      SELECT 1
      FROM auth.ApplicationGroupRoles existing
      WHERE existing.Name = 'Distribuidores'
        AND ((existing.GroupHeaderId IS NULL AND gr.GroupHeaderId IS NULL)
             OR existing.GroupHeaderId = gr.GroupHeaderId)
  );

INSERT INTO auth.ApplicationGroupRolePermissions (PermissionId, GroupRoleId, IsActive, CreationTime, CreatorUser)
SELECT p.Id, dest.Id, 1, @now, 'MIGRATION'
FROM auth.ApplicationGroupRoles dest
CROSS JOIN #DistributorPermissions p
WHERE dest.Name = 'Distribuidores'
  AND dest.CreatorUser = 'MIGRATION'
  AND NOT EXISTS (
      SELECT 1
      FROM auth.ApplicationGroupRolePermissions existing
      WHERE existing.GroupRoleId = dest.Id
        AND existing.PermissionId = p.Id
  );

INSERT INTO auth.ApplicationTenantPermissions (TenantId, PermissionId, IsActive, CreationTime, CreatorUser)
SELECT tp.TenantId, dest.Id, 1, @now, 'MIGRATION'
FROM auth.ApplicationTenantPermissions tp
INNER JOIN AspNetRoles source ON source.Id = tp.PermissionId
INNER JOIN AspNetRoles dest ON dest.Name = REPLACE(source.Name, 'DATA.ROUTE', 'DATA.DISTRIBUTOR')
WHERE source.Name LIKE 'DATA.ROUTE%'
  AND dest.Name LIKE 'DATA.DISTRIBUTOR%'
  AND NOT EXISTS (
      SELECT 1
      FROM auth.ApplicationTenantPermissions existing
      WHERE existing.TenantId = tp.TenantId
        AND existing.PermissionId = dest.Id
  );

INSERT INTO auth.ApplicationRolePermissions (RolId, PermissionId, TenantId, IsActive, CreationTime, CreatorUser)
SELECT rp.RolId, dest.Id, rp.TenantId, 1, @now, 'MIGRATION'
FROM auth.ApplicationRolePermissions rp
INNER JOIN AspNetRoles source ON source.Id = rp.PermissionId
INNER JOIN AspNetRoles dest ON dest.Name = REPLACE(source.Name, 'DATA.ROUTE', 'DATA.DISTRIBUTOR')
WHERE source.Name LIKE 'DATA.ROUTE%'
  AND dest.Name LIKE 'DATA.DISTRIBUTOR%'
  AND NOT EXISTS (
      SELECT 1
      FROM auth.ApplicationRolePermissions existing
      WHERE existing.RolId = rp.RolId
        AND existing.PermissionId = dest.Id
        AND ((existing.TenantId IS NULL AND rp.TenantId IS NULL) OR existing.TenantId = rp.TenantId)
  );

DROP TABLE #DistributorPermissions;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE rp
FROM auth.ApplicationRolePermissions rp
INNER JOIN AspNetRoles r ON r.Id = rp.PermissionId
WHERE r.Name LIKE 'DATA.DISTRIBUTOR%';

DELETE tp
FROM auth.ApplicationTenantPermissions tp
INNER JOIN AspNetRoles r ON r.Id = tp.PermissionId
WHERE r.Name LIKE 'DATA.DISTRIBUTOR%';

DELETE grp
FROM auth.ApplicationGroupRolePermissions grp
INNER JOIN AspNetRoles r ON r.Id = grp.PermissionId
WHERE r.Name LIKE 'DATA.DISTRIBUTOR%';

DELETE FROM auth.ApplicationGroupRoles
WHERE Name = 'Distribuidores' AND CreatorUser = 'MIGRATION';

DELETE FROM AspNetRoles
WHERE Name LIKE 'DATA.DISTRIBUTOR%';
");

            migrationBuilder.DropTable(
                name: "ProductDistributorPrices",
                schema: "data");

            migrationBuilder.DropTable(
                name: "Distributors",
                schema: "data");
        }
    }
}
