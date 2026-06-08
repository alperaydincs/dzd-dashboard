using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RestrictHistoryCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGradeHistories_Users_UserId",
                table: "UserGradeHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSalaryHistories_Users_UserId",
                table: "UserSalaryHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGradeHistories_Users_UserId",
                table: "UserGradeHistories",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGradeHistories_Users_UserId",
                table: "UserGradeHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSalaryHistories_Users_UserId",
                table: "UserSalaryHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGradeHistories_Users_UserId",
                table: "UserGradeHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSalaryHistories_Users_UserId",
                table: "UserSalaryHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
