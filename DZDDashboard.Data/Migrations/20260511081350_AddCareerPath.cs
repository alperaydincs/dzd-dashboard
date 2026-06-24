using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    public partial class AddCareerPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareerMapRules_Jobs_JobId",
                table: "CareerMapRules");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "CareerMapRules",
                newName: "CareerPathId");

            migrationBuilder.RenameIndex(
                name: "IX_CareerMapRules_JobId_Grade",
                table: "CareerMapRules",
                newName: "IX_CareerMapRules_CareerPathId_Grade");

            migrationBuilder.AlterColumn<decimal>(
                name: "SalaryIncreasePercent",
                table: "CareerMapRules",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CareerPaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserGroupId = table.Column<int>(type: "int", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerPaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CareerPaths_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CareerPaths_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareerPaths_ModifiedById",
                table: "CareerPaths",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CareerPaths_UserGroupId",
                table: "CareerPaths",
                column: "UserGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareerMapRules_CareerPaths_CareerPathId",
                table: "CareerMapRules",
                column: "CareerPathId",
                principalTable: "CareerPaths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareerMapRules_CareerPaths_CareerPathId",
                table: "CareerMapRules");

            migrationBuilder.DropTable(
                name: "CareerPaths");

            migrationBuilder.RenameColumn(
                name: "CareerPathId",
                table: "CareerMapRules",
                newName: "JobId");

            migrationBuilder.RenameIndex(
                name: "IX_CareerMapRules_CareerPathId_Grade",
                table: "CareerMapRules",
                newName: "IX_CareerMapRules_JobId_Grade");

            migrationBuilder.AlterColumn<decimal>(
                name: "SalaryIncreasePercent",
                table: "CareerMapRules",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CareerMapRules_Jobs_JobId",
                table: "CareerMapRules",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
