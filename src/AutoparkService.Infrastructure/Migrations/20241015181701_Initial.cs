using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoparkService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Odometer = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Insurances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Series = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Number = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    VehicleType = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Provider = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insurances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Insurances_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Odometer = table.Column<long>(type: "bigint", nullable: false),
                    ServiceCenter = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalPassports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FirstNameLatin = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastNameLatin = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Patronymic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SAICode = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    LicensePlate = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Brand = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreationYear = table.Column<long>(type: "bigint", nullable: false),
                    Color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VIN = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    VehicleType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MaxWeight = table.Column<long>(type: "bigint", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalPassports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalPassports_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Insurances_Series_Number",
                table: "Insurances",
                columns: new[] { "Series", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Insurances_VehicleId",
                table: "Insurances",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_VehicleId",
                table: "MaintenanceRecords",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Number",
                table: "Permissions",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_VehicleId",
                table: "Permissions",
                column: "VehicleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalPassports_LicensePlate",
                table: "TechnicalPassports",
                column: "LicensePlate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalPassports_Number",
                table: "TechnicalPassports",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalPassports_VehicleId",
                table: "TechnicalPassports",
                column: "VehicleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalPassports_VIN",
                table: "TechnicalPassports",
                column: "VIN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Insurances");

            migrationBuilder.DropTable(
                name: "MaintenanceRecords");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "TechnicalPassports");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
