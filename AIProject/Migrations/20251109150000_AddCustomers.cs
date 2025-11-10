using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIProject.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    AlternatePhoneNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StateOrProvince = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreferredContactMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreferredContactTime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreferredLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LoyaltyNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MarketingOptIn = table.Column<bool>(type: "bit", nullable: false),
                    IsVip = table.Column<bool>(type: "bit", nullable: false),
                    Anniversary = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LoyaltyNumber",
                table: "Customers",
                column: "LoyaltyNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
