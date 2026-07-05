using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPositionHistoryTeamDepartmentSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPositionHistories_Departments_DepartmentId",
                table: "UserPositionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPositionHistories_Teams_TeamId",
                table: "UserPositionHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositionHistories_Departments_DepartmentId",
                table: "UserPositionHistories",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositionHistories_Teams_TeamId",
                table: "UserPositionHistories",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPositionHistories_Departments_DepartmentId",
                table: "UserPositionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPositionHistories_Teams_TeamId",
                table: "UserPositionHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositionHistories_Departments_DepartmentId",
                table: "UserPositionHistories",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositionHistories_Teams_TeamId",
                table: "UserPositionHistories",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
