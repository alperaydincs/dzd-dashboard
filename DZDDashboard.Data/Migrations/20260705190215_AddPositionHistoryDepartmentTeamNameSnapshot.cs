using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionHistoryDepartmentTeamNameSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "UserPositionHistories",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamName",
                table: "UserPositionHistories",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE h
                SET h.DepartmentName = d.Name
                FROM UserPositionHistories h
                JOIN Departments d ON d.Id = h.DepartmentId
                WHERE h.DepartmentId IS NOT NULL;

                UPDATE h
                SET h.TeamName = t.Name
                FROM UserPositionHistories h
                JOIN Teams t ON t.Id = h.TeamId
                WHERE h.TeamId IS NOT NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "UserPositionHistories");

            migrationBuilder.DropColumn(
                name: "TeamName",
                table: "UserPositionHistories");
        }
    }
}
