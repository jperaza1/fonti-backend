using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cod.persistence.Migrations
{
    public partial class removingtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserProfiles_Cafeterias_CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Routes_RouteId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_PurchaseOrders_PurchaseOrderId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Types_CategoryId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Vendors_VendorId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "CustomerCafeterias",
                schema: "data");

            migrationBuilder.DropTable(
                name: "DishesCafeteria",
                schema: "salepoint");

            migrationBuilder.DropTable(
                name: "EmployeeChildren",
                schema: "rrhh");

            migrationBuilder.DropTable(
                name: "EmployeeContractBeneficiaries",
                schema: "rrhh");

            migrationBuilder.DropTable(
                name: "EmployeePersonalReferences",
                schema: "rrhh");

            migrationBuilder.DropTable(
                name: "ProductRoutePrices",
                schema: "data");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDetails",
                schema: "operation");

            migrationBuilder.DropTable(
                name: "Registers",
                schema: "data");

            migrationBuilder.DropTable(
                name: "SaleDetails",
                schema: "operation");

            migrationBuilder.DropTable(
                name: "SideItems",
                schema: "salepoint");

            migrationBuilder.DropTable(
                name: "VacationPeriod",
                schema: "rrhh");

            migrationBuilder.DropTable(
                name: "VacationPolicies",
                schema: "data");

            migrationBuilder.DropTable(
                name: "VacationRequested",
                schema: "rrhh");

            migrationBuilder.DropTable(
                name: "WorkExperience",
                schema: "rrhh");

            migrationBuilder.DropTable(
                name: "Dishes",
                schema: "salepoint");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "operation");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "data");

            migrationBuilder.DropTable(
                name: "Sales",
                schema: "operation");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "rrhh");

            migrationBuilder.DropTable(
                name: "Routes",
                schema: "data");

            migrationBuilder.DropTable(
                name: "Cafeterias",
                schema: "data");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CafeteriaId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CategoryId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_PurchaseOrderId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_RouteId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserProfiles_CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.DropColumn(
                name: "CafeteriaId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "FileUploadedAt",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "FileUploadedUser",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "HasFile",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceType",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "IsInvoice",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "RouteId",
                schema: "operation",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "auth",
                table: "ApplicationUserProfiles");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                schema: "operation",
                table: "Invoices",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_VendorId",
                schema: "operation",
                table: "Invoices",
                newName: "IX_Invoices_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                schema: "operation",
                table: "Invoices",
                column: "CustomerId",
                principalSchema: "data",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                schema: "operation",
                table: "Invoices");

            migrationBuilder.EnsureSchema(
                name: "salepoint");

            migrationBuilder.EnsureSchema(
                name: "rrhh");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "operation",
                table: "Invoices",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_CustomerId",
                schema: "operation",
                table: "Invoices",
                newName: "IX_Invoices_VendorId");

            migrationBuilder.AddColumn<int>(
                name: "CafeteriaId",
                schema: "operation",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "operation",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                schema: "operation",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FileUploadedAt",
                schema: "operation",
                table: "Invoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUploadedUser",
                schema: "operation",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasFile",
                schema: "operation",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceType",
                schema: "operation",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                schema: "operation",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                schema: "operation",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInvoice",
                schema: "operation",
                table: "Expenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                schema: "operation",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "auth",
                table: "ApplicationUserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "auth",
                table: "ApplicationUserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cafeterias",
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
                    InventoryCorelative = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeRequisitionCorelative = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderCorelative = table.Column<int>(type: "int", nullable: false),
                    SpecialServicesCorelative = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cafeterias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                schema: "salepoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DishType = table.Column<int>(type: "int", nullable: false),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SideItems",
                schema: "salepoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_SideItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VacationPolicies",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayPerYear = table.Column<int>(type: "int", nullable: false),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromYear = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCafeterias",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CafeteriaId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCafeterias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerCafeterias_Cafeterias_CafeteriaId",
                        column: x => x.CafeteriaId,
                        principalSchema: "data",
                        principalTable: "Cafeterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerCafeterias_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "data",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "rrhh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BirthPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CafeteriaId = table.Column<int>(type: "int", nullable: false),
                    ChronicConditionDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStudyInstitution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationField = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationInstitution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationLevel = table.Column<int>(type: "int", nullable: false),
                    EmergencyContactAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactRelationship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmploymentStatus = table.Column<int>(type: "int", nullable: false),
                    EmploymentType = table.Column<int>(type: "int", nullable: false),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FatherBirthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FatherFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherProfession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    HasChildren = table.Column<bool>(type: "bit", nullable: false),
                    HasChronicCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasFamilyAtCompany = table.Column<bool>(type: "bit", nullable: false),
                    HealthDisclosureDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HousingType = table.Column<int>(type: "int", nullable: false),
                    Identification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsCurrentlyStudying = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<int>(type: "int", nullable: false),
                    MotherBirthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotherFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherProfession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayFrequency = table.Column<int>(type: "int", nullable: false),
                    PermanentStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    Profession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    Shift = table.Column<int>(type: "int", nullable: false),
                    SpouseBirthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SpouseFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalVacation = table.Column<int>(type: "int", nullable: false),
                    TrialPeriodDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UniformSizeId = table.Column<int>(type: "int", nullable: false),
                    YearsAtResidence = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Cafeterias_CafeteriaId",
                        column: x => x.CafeteriaId,
                        principalSchema: "data",
                        principalTable: "Cafeterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Types_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "data",
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Types_UniformSizeId",
                        column: x => x.UniformSizeId,
                        principalSchema: "data",
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CafeteriaId = table.Column<int>(type: "int", nullable: false),
                    CanceledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CanceledUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    ProcessingAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessingUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonCanceled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonRejection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: false),
                    TypeInoice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Cafeterias_CafeteriaId",
                        column: x => x.CafeteriaId,
                        principalSchema: "data",
                        principalTable: "Cafeterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "data",
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registers",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CafeteriaId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UseInInventory = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registers_Cafeterias_CafeteriaId",
                        column: x => x.CafeteriaId,
                        principalSchema: "data",
                        principalTable: "Cafeterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishesCafeteria",
                schema: "salepoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CafeteriaId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DishId = table.Column<int>(type: "int", nullable: false),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreeAddOnsQuantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoAddOns = table.Column<bool>(type: "bit", nullable: false),
                    PriceWithCarnet = table.Column<double>(type: "float", nullable: false),
                    PriceWithOutCarnet = table.Column<double>(type: "float", nullable: false),
                    SaleTypeDish = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishesCafeteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishesCafeteria_Cafeterias_CafeteriaId",
                        column: x => x.CafeteriaId,
                        principalSchema: "data",
                        principalTable: "Cafeterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishesCafeteria_Dishes_DishId",
                        column: x => x.DishId,
                        principalSchema: "salepoint",
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRoutePrices",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRoutePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRoutePrices_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "data",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRoutePrices_Routes_RouteId",
                        column: x => x.RouteId,
                        principalSchema: "data",
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaleType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Routes_RouteId",
                        column: x => x.RouteId,
                        principalSchema: "data",
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeChildren",
                schema: "rrhh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeChildren", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeChildren_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "rrhh",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeContractBeneficiaries",
                schema: "rrhh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeContractBeneficiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeContractBeneficiaries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "rrhh",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePersonalReferences",
                schema: "rrhh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePersonalReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalReferences_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "rrhh",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacationPeriod",
                schema: "rrhh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntitledDays = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationPeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationPeriod_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "rrhh",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacationRequested",
                schema: "rrhh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationRequested", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationRequested_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "rrhh",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkExperience",
                schema: "rrhh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonForLeaving = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperience_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "rrhh",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetails",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    CostRecived = table.Column<double>(type: "float", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailStatus = table.Column<int>(type: "int", nullable: false),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: false),
                    DiscountRecived = table.Column<double>(type: "float", nullable: false),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    QuantityRecived = table.Column<double>(type: "float", nullable: false),
                    SubTotal = table.Column<double>(type: "float", nullable: false),
                    SubTotalRecived = table.Column<double>(type: "float", nullable: false),
                    Tax = table.Column<double>(type: "float", nullable: false),
                    TaxPercentage = table.Column<double>(type: "float", nullable: false),
                    TaxRecived = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    TotalRecived = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "data",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "operation",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleDetails",
                schema: "operation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "data",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleDetails_Sales_SaleId",
                        column: x => x.SaleId,
                        principalSchema: "operation",
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CafeteriaId",
                schema: "operation",
                table: "Invoices",
                column: "CafeteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CategoryId",
                schema: "operation",
                table: "Invoices",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PurchaseOrderId",
                schema: "operation",
                table: "Invoices",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_RouteId",
                schema: "operation",
                table: "Expenses",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserProfiles_CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles",
                column: "CafeteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCafeterias_CafeteriaId",
                schema: "data",
                table: "CustomerCafeterias",
                column: "CafeteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCafeterias_CustomerId",
                schema: "data",
                table: "CustomerCafeterias",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_DishesCafeteria_CafeteriaId",
                schema: "salepoint",
                table: "DishesCafeteria",
                column: "CafeteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_DishesCafeteria_DishId",
                schema: "salepoint",
                table: "DishesCafeteria",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeChildren_EmployeeId",
                schema: "rrhh",
                table: "EmployeeChildren",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContractBeneficiaries_EmployeeId",
                schema: "rrhh",
                table: "EmployeeContractBeneficiaries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalReferences_EmployeeId",
                schema: "rrhh",
                table: "EmployeePersonalReferences",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CafeteriaId",
                schema: "rrhh",
                table: "Employees",
                column: "CafeteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionId",
                schema: "rrhh",
                table: "Employees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UniformSizeId",
                schema: "rrhh",
                table: "Employees",
                column: "UniformSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRoutePrices_ProductId",
                schema: "data",
                table: "ProductRoutePrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRoutePrices_RouteId",
                schema: "data",
                table: "ProductRoutePrices",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_ProductId",
                schema: "operation",
                table: "PurchaseOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_PurchaseOrderId",
                schema: "operation",
                table: "PurchaseOrderDetails",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CafeteriaId",
                schema: "operation",
                table: "PurchaseOrders",
                column: "CafeteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorId",
                schema: "operation",
                table: "PurchaseOrders",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CafeteriaId",
                schema: "data",
                table: "Registers",
                column: "CafeteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_ProductId",
                schema: "operation",
                table: "SaleDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_SaleId",
                schema: "operation",
                table: "SaleDetails",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_RouteId",
                schema: "operation",
                table: "Sales",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationPeriod_EmployeeId",
                schema: "rrhh",
                table: "VacationPeriod",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequested_EmployeeId",
                schema: "rrhh",
                table: "VacationRequested",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperience_EmployeeId",
                schema: "rrhh",
                table: "WorkExperience",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserProfiles_Cafeterias_CafeteriaId",
                schema: "auth",
                table: "ApplicationUserProfiles",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Routes_RouteId",
                schema: "operation",
                table: "Expenses",
                column: "RouteId",
                principalSchema: "data",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Cafeterias_CafeteriaId",
                schema: "operation",
                table: "Invoices",
                column: "CafeteriaId",
                principalSchema: "data",
                principalTable: "Cafeterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_PurchaseOrders_PurchaseOrderId",
                schema: "operation",
                table: "Invoices",
                column: "PurchaseOrderId",
                principalSchema: "operation",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Types_CategoryId",
                schema: "operation",
                table: "Invoices",
                column: "CategoryId",
                principalSchema: "data",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Vendors_VendorId",
                schema: "operation",
                table: "Invoices",
                column: "VendorId",
                principalSchema: "data",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
