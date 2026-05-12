using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorRequirementFields : Migration
    {
        /// <inheritdoc />
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

        /// <inheritdoc />
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
