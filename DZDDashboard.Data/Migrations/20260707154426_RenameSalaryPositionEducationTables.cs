using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameSalaryPositionEducationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEducationHistories_Users_UserId",
                table: "UserEducationHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPositionHistories_Users_UserId",
                table: "UserPositionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSalaryHistories_Users_UserId",
                table: "UserSalaryHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSalaryHistories",
                table: "UserSalaryHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPositionHistories",
                table: "UserPositionHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEducationHistories",
                table: "UserEducationHistories");

            migrationBuilder.RenameTable(
                name: "UserSalaryHistories",
                newName: "UserSalaries");

            migrationBuilder.RenameTable(
                name: "UserPositionHistories",
                newName: "UserPositions");

            migrationBuilder.RenameTable(
                name: "UserEducationHistories",
                newName: "UserEducations");

            migrationBuilder.RenameIndex(
                name: "IX_UserSalaryHistories_UserId_StartDate",
                table: "UserSalaries",
                newName: "IX_UserSalaries_UserId_StartDate");

            migrationBuilder.RenameIndex(
                name: "IX_UserPositionHistories_UserId",
                table: "UserPositions",
                newName: "IX_UserPositions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEducationHistories_UserId",
                table: "UserEducations",
                newName: "IX_UserEducations_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSalaries",
                table: "UserSalaries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEducations",
                table: "UserEducations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEducations_Users_UserId",
                table: "UserEducations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositions_Users_UserId",
                table: "UserPositions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSalaries_Users_UserId",
                table: "UserSalaries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEducations_Users_UserId",
                table: "UserEducations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPositions_Users_UserId",
                table: "UserPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSalaries_Users_UserId",
                table: "UserSalaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSalaries",
                table: "UserSalaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEducations",
                table: "UserEducations");

            migrationBuilder.RenameTable(
                name: "UserSalaries",
                newName: "UserSalaryHistories");

            migrationBuilder.RenameTable(
                name: "UserPositions",
                newName: "UserPositionHistories");

            migrationBuilder.RenameTable(
                name: "UserEducations",
                newName: "UserEducationHistories");

            migrationBuilder.RenameIndex(
                name: "IX_UserSalaries_UserId_StartDate",
                table: "UserSalaryHistories",
                newName: "IX_UserSalaryHistories_UserId_StartDate");

            migrationBuilder.RenameIndex(
                name: "IX_UserPositions_UserId",
                table: "UserPositionHistories",
                newName: "IX_UserPositionHistories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEducations_UserId",
                table: "UserEducationHistories",
                newName: "IX_UserEducationHistories_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSalaryHistories",
                table: "UserSalaryHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPositionHistories",
                table: "UserPositionHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEducationHistories",
                table: "UserEducationHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEducationHistories_Users_UserId",
                table: "UserEducationHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositionHistories_Users_UserId",
                table: "UserPositionHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSalaryHistories_Users_UserId",
                table: "UserSalaryHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
