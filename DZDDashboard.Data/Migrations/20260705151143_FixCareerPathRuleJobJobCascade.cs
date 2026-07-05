using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCareerPathRuleJobJobCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareerPathRuleJobs_Jobs_JobId",
                table: "CareerPathRuleJobs");

            migrationBuilder.AddForeignKey(
                name: "FK_CareerPathRuleJobs_Jobs_JobId",
                table: "CareerPathRuleJobs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareerPathRuleJobs_Jobs_JobId",
                table: "CareerPathRuleJobs");

            migrationBuilder.AddForeignKey(
                name: "FK_CareerPathRuleJobs_Jobs_JobId",
                table: "CareerPathRuleJobs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
