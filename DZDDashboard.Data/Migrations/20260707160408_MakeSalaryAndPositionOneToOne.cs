using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeSalaryAndPositionOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSalaries_UserId_StartDate",
                table: "UserSalaries");

            migrationBuilder.DropIndex(
                name: "IX_UserPositions_UserId",
                table: "UserPositions");

            migrationBuilder.CreateIndex(
                name: "IX_UserSalaries_UserId",
                table: "UserSalaries",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPositions_UserId",
                table: "UserPositions",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSalaries_UserId",
                table: "UserSalaries");

            migrationBuilder.DropIndex(
                name: "IX_UserPositions_UserId",
                table: "UserPositions");

            migrationBuilder.CreateIndex(
                name: "IX_UserSalaries_UserId_StartDate",
                table: "UserSalaries",
                columns: new[] { "UserId", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_UserPositions_UserId",
                table: "UserPositions",
                column: "UserId");
        }
    }
}
