using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIProject.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerMasters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenderOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenderOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreferredContactMethodOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferredContactMethodOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreferredLanguageOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferredLanguageOptions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GenderOptions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Female" },
                    { 2, "Male" },
                    { 3, "Non-binary" },
                    { 4, "Prefer not to say" },
                    { 5, "Other" }
                });

            migrationBuilder.InsertData(
                table: "PreferredContactMethodOptions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Email" },
                    { 2, "Phone" },
                    { 3, "SMS" },
                    { 4, "Video call" },
                    { 5, "In-person" }
                });

            migrationBuilder.InsertData(
                table: "PreferredLanguageOptions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "English" },
                    { 2, "Spanish" },
                    { 3, "French" },
                    { 4, "German" },
                    { 5, "Mandarin" }
                });

            migrationBuilder.AddColumn<int>(
                name: "GenderOptionId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreferredContactMethodOptionId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreferredLanguageOptionId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_GenderOptionId",
                table: "Customers",
                column: "GenderOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PreferredContactMethodOptionId",
                table: "Customers",
                column: "PreferredContactMethodOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PreferredLanguageOptionId",
                table: "Customers",
                column: "PreferredLanguageOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_GenderOptions_GenderOptionId",
                table: "Customers",
                column: "GenderOptionId",
                principalTable: "GenderOptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_PreferredContactMethodOptions_PreferredContactMethodOptionId",
                table: "Customers",
                column: "PreferredContactMethodOptionId",
                principalTable: "PreferredContactMethodOptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_PreferredLanguageOptions_PreferredLanguageOptionId",
                table: "Customers",
                column: "PreferredLanguageOptionId",
                principalTable: "PreferredLanguageOptions",
                principalColumn: "Id");

            migrationBuilder.Sql(@"UPDATE c SET GenderOptionId = g.Id FROM Customers c INNER JOIN GenderOptions g ON g.Name = c.Gender WHERE c.Gender IS NOT NULL");

            migrationBuilder.Sql(@"UPDATE c SET PreferredContactMethodOptionId = m.Id FROM Customers c INNER JOIN PreferredContactMethodOptions m ON m.Name = c.PreferredContactMethod WHERE c.PreferredContactMethod IS NOT NULL");

            migrationBuilder.Sql(@"UPDATE c SET PreferredLanguageOptionId = l.Id FROM Customers c INNER JOIN PreferredLanguageOptions l ON l.Name = c.PreferredLanguage WHERE c.PreferredLanguage IS NOT NULL");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PreferredContactMethod",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PreferredLanguage",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredContactMethod",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredLanguage",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.Sql(@"UPDATE c SET Gender = g.Name FROM Customers c LEFT JOIN GenderOptions g ON g.Id = c.GenderOptionId WHERE c.GenderOptionId IS NOT NULL");

            migrationBuilder.Sql(@"UPDATE c SET PreferredContactMethod = m.Name FROM Customers c LEFT JOIN PreferredContactMethodOptions m ON m.Id = c.PreferredContactMethodOptionId WHERE c.PreferredContactMethodOptionId IS NOT NULL");

            migrationBuilder.Sql(@"UPDATE c SET PreferredLanguage = l.Name FROM Customers c LEFT JOIN PreferredLanguageOptions l ON l.Id = c.PreferredLanguageOptionId WHERE c.PreferredLanguageOptionId IS NOT NULL");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_GenderOptions_GenderOptionId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_PreferredContactMethodOptions_PreferredContactMethodOptionId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_PreferredLanguageOptions_PreferredLanguageOptionId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_GenderOptionId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_PreferredContactMethodOptionId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_PreferredLanguageOptionId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "GenderOptionId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PreferredContactMethodOptionId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PreferredLanguageOptionId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "GenderOptions");

            migrationBuilder.DropTable(
                name: "PreferredContactMethodOptions");

            migrationBuilder.DropTable(
                name: "PreferredLanguageOptions");
        }
    }
}
