using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class SplitBenefitPaymentIntoTph : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BenefitPaymentHistory");

            migrationBuilder.CreateTable(
                name: "HealthInsuranceBenefitHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    BenefitType = table.Column<string>(type: "text", nullable: false),
                    BenefitName = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProviderName = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthInsuranceBenefitHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "OtherBenefitHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    BenefitType = table.Column<string>(type: "text", nullable: false),
                    BenefitName = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProviderName = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherBenefitHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "PensionBenefitHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    BenefitType = table.Column<string>(type: "text", nullable: false),
                    BenefitName = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProviderName = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    EmployeeContributionAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    EmployerContributionAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    PolicyNumber = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PensionBenefitHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthInsuranceBenefitHistory_Id",
                table: "HealthInsuranceBenefitHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OtherBenefitHistory_Id",
                table: "OtherBenefitHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PensionBenefitHistory_Id",
                table: "PensionBenefitHistory",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthInsuranceBenefitHistory");

            migrationBuilder.DropTable(
                name: "OtherBenefitHistory");

            migrationBuilder.DropTable(
                name: "PensionBenefitHistory");

            migrationBuilder.CreateTable(
                name: "BenefitPaymentHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    BenefitName = table.Column<string>(type: "text", nullable: true),
                    BenefitType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    EmployeeContributionAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    EmployerContributionAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    PolicyNumber = table.Column<string>(type: "text", nullable: true),
                    ProviderName = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitPaymentHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitPaymentHistory_Id",
                table: "BenefitPaymentHistory",
                column: "Id");
        }
    }
}
