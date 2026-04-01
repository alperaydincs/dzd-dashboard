using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeEducationToHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssociateDegreeProgramName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AssociateDegreeUniversityName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BachelorsGraduatedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BachelorsProgramName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BachelorsUniversityName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DoctoratePhdStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DoctorateProgramName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DoctorateUniversityName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EducationStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HighSchoolName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HighestEducationLevel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MastersGraduatedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MastersProgramName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MastersUniversityName",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserEducationHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Institution = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Program = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    GraduationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEducationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEducationHistories_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserEducationHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEducationHistories_ModifiedById",
                table: "UserEducationHistories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserEducationHistories_UserId",
                table: "UserEducationHistories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEducationHistories");

            migrationBuilder.AddColumn<string>(
                name: "AssociateDegreeProgramName",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssociateDegreeUniversityName",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BachelorsGraduatedDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BachelorsProgramName",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BachelorsUniversityName",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctoratePhdStatus",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorateProgramName",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorateUniversityName",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EducationStatus",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighSchoolName",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighestEducationLevel",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MastersGraduatedDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MastersProgramName",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MastersUniversityName",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
