using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    public partial class OneUserPerOrgPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_OrganizationPositionId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationPositionId",
                table: "Users",
                column: "OrganizationPositionId",
                unique: true,
                filter: "[OrganizationPositionId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_OrganizationPositionId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationPositionId",
                table: "Users",
                column: "OrganizationPositionId");
        }
    }
}
