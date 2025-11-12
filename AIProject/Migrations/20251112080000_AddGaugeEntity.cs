using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIProject.Migrations
{
    /// <inheritdoc />
    public partial class AddGaugeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gauges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Range = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ManufactureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModelNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EquipmentId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CalibrationFrequency = table.Column<int>(type: "int", nullable: true),
                    CalibrationFrequencyType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CalibrationProcedure = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NextCalibrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CalibrationResult = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CalibrationCertificateNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ReceivedFrom = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CalibratedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CalibrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LocationCalibrated = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LocationOfRecords = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaintenanceDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Owner = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    GaugeType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Uut = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gauges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gauges_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gauges_CustomerId",
                table: "Gauges",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gauges");
        }
    }
}
