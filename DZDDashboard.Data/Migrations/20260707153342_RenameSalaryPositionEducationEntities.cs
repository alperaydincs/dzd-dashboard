using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameSalaryPositionEducationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationHistoryHistory");

            migrationBuilder.DropTable(
                name: "PositionHistoryHistory");

            migrationBuilder.DropTable(
                name: "SalaryHistoryHistory");

            migrationBuilder.CreateTable(
                name: "EducationHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EducationLevel = table.Column<string>(type: "text", nullable: true),
                    Institution = table.Column<string>(type: "text", nullable: true),
                    Program = table.Column<string>(type: "text", nullable: true),
                    GraduationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "PositionHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    JobTitle = table.Column<string>(type: "text", nullable: true),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    DepartmentName = table.Column<string>(type: "text", nullable: true),
                    TeamName = table.Column<string>(type: "text", nullable: true),
                    Grade = table.Column<int>(type: "integer", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangeType = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "SalaryHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    PayType = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    PayrollCycle = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    NotesModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationHistory_Id",
                table: "EducationHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PositionHistory_Id",
                table: "PositionHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryHistory_Id",
                table: "SalaryHistory",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationHistory");

            migrationBuilder.DropTable(
                name: "PositionHistory");

            migrationBuilder.DropTable(
                name: "SalaryHistory");

            migrationBuilder.CreateTable(
                name: "EducationHistoryHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EducationLevel = table.Column<string>(type: "text", nullable: true),
                    GraduationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Institution = table.Column<string>(type: "text", nullable: true),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Program = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationHistoryHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "PositionHistoryHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChangeType = table.Column<string>(type: "text", nullable: true),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DepartmentName = table.Column<string>(type: "text", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Grade = table.Column<int>(type: "integer", nullable: true),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    JobTitle = table.Column<string>(type: "text", nullable: true),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TeamName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionHistoryHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "SalaryHistoryHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    NotesModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PayType = table.Column<string>(type: "text", nullable: false),
                    PayrollCycle = table.Column<string>(type: "text", nullable: true),
                    Period = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryHistoryHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationHistoryHistory_Id",
                table: "EducationHistoryHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PositionHistoryHistory_Id",
                table: "PositionHistoryHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryHistoryHistory_Id",
                table: "SalaryHistoryHistory",
                column: "Id");
        }
    }
}
