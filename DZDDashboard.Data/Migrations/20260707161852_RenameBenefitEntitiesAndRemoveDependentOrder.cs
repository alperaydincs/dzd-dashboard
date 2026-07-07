using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameBenefitEntitiesAndRemoveDependentOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBenefitDependents_UserBenefitRecords_BenefitRecordId",
                table: "UserBenefitDependents");

            migrationBuilder.DropTable(
                name: "BenefitDependentHistory");

            migrationBuilder.DropTable(
                name: "BenefitRecordHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserBenefitDependents_BenefitRecordId",
                table: "UserBenefitDependents");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "UserBenefitDependents");

            migrationBuilder.RenameColumn(
                name: "BenefitRecordId",
                table: "UserBenefitDependents",
                newName: "BenefitPaymentId");

            migrationBuilder.CreateTable(
                name: "BenefitPaymentDependentHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    DependentName = table.Column<string>(type: "text", nullable: true),
                    RelationType = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BenefitPaymentId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitPaymentDependentHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "BenefitPaymentHistory",
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
                    table.PrimaryKey("PK_BenefitPaymentHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefitDependents_BenefitPaymentId",
                table: "UserBenefitDependents",
                column: "BenefitPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitPaymentDependentHistory_Id",
                table: "BenefitPaymentDependentHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitPaymentHistory_Id",
                table: "BenefitPaymentHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBenefitDependents_UserBenefitRecords_BenefitPaymentId",
                table: "UserBenefitDependents",
                column: "BenefitPaymentId",
                principalTable: "UserBenefitRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBenefitDependents_UserBenefitRecords_BenefitPaymentId",
                table: "UserBenefitDependents");

            migrationBuilder.DropTable(
                name: "BenefitPaymentDependentHistory");

            migrationBuilder.DropTable(
                name: "BenefitPaymentHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserBenefitDependents_BenefitPaymentId",
                table: "UserBenefitDependents");

            migrationBuilder.RenameColumn(
                name: "BenefitPaymentId",
                table: "UserBenefitDependents",
                newName: "BenefitRecordId");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "UserBenefitDependents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BenefitDependentHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    BenefitRecordId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DependentName = table.Column<string>(type: "text", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    RelationType = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitDependentHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "BenefitRecordHistory",
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
                    table.PrimaryKey("PK_BenefitRecordHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefitDependents_BenefitRecordId",
                table: "UserBenefitDependents",
                column: "BenefitRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitDependentHistory_Id",
                table: "BenefitDependentHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitRecordHistory_Id",
                table: "BenefitRecordHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBenefitDependents_UserBenefitRecords_BenefitRecordId",
                table: "UserBenefitDependents",
                column: "BenefitRecordId",
                principalTable: "UserBenefitRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
