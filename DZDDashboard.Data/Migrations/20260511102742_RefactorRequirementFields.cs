using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    public partial class RefactorRequirementFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssesmentCenterApplication",
                table: "CareerMapRules");

            migrationBuilder.RenameColumn(
                name: "Evaluation",
                table: "CareerMapRules",
                newName: "AssessmentCenterApplication");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssessmentCenterApplication",
                table: "CareerMapRules",
                newName: "Evaluation");

            migrationBuilder.AddColumn<bool>(
                name: "AssesmentCenterApplication",
                table: "CareerMapRules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
