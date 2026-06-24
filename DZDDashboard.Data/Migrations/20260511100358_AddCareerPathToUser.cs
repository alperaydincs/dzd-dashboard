using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    public partial class AddCareerPathToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CareerPathId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CareerPathId",
                table: "Users",
                column: "CareerPathId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CareerPaths_CareerPathId",
                table: "Users",
                column: "CareerPathId",
                principalTable: "CareerPaths",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_CareerPaths_CareerPathId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CareerPathId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CareerPathId",
                table: "Users");
        }
    }
}
