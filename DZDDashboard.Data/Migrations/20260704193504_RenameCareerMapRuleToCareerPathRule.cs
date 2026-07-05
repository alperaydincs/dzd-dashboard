using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameCareerMapRuleToCareerPathRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CareerMapRuleId",
                table: "CareerMapRulePositions",
                newName: "CareerPathRuleId");

            migrationBuilder.RenameTable(
                name: "CareerMapRulePositions",
                newName: "CareerPathRuleJobs");

            migrationBuilder.RenameTable(
                name: "CareerMapRules",
                newName: "CareerPathRules");

            migrationBuilder.RenameIndex(
                name: "IX_CareerMapRulePositions_JobId",
                table: "CareerPathRuleJobs",
                newName: "IX_CareerPathRuleJobs_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_CareerMapRules_CareerPathId_Grade",
                table: "CareerPathRules",
                newName: "IX_CareerPathRules_CareerPathId_Grade");

            migrationBuilder.RenameIndex(
                name: "IX_CareerMapRules_ModifiedById",
                table: "CareerPathRules",
                newName: "IX_CareerPathRules_ModifiedById");

            migrationBuilder.Sql("EXEC sp_rename N'dbo.PK_CareerMapRulePositions', N'PK_CareerPathRuleJobs';");
            migrationBuilder.Sql("EXEC sp_rename N'dbo.PK_CareerMapRules', N'PK_CareerPathRules';");

            migrationBuilder.Sql("EXEC sp_rename N'dbo.FK_CareerMapRulePositions_CareerMapRules_CareerMapRuleId', N'FK_CareerPathRuleJobs_CareerPathRules_CareerPathRuleId';");
            migrationBuilder.Sql("EXEC sp_rename N'dbo.FK_CareerMapRulePositions_Jobs_JobId', N'FK_CareerPathRuleJobs_Jobs_JobId';");
            migrationBuilder.Sql("EXEC sp_rename N'dbo.FK_CareerMapRules_CareerPaths_CareerPathId', N'FK_CareerPathRules_CareerPaths_CareerPathId';");
            migrationBuilder.Sql("EXEC sp_rename N'dbo.FK_CareerMapRules_Users_ModifiedById', N'FK_CareerPathRules_Users_ModifiedById';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("EXEC sp_rename N'dbo.FK_CareerPathRuleJobs_CareerPathRules_CareerPathRuleId', N'FK_CareerMapRulePositions_CareerMapRules_CareerMapRuleId';");
            migrationBuilder.Sql("EXEC sp_rename N'dbo.FK_CareerPathRuleJobs_Jobs_JobId', N'FK_CareerMapRulePositions_Jobs_JobId';");
            migrationBuilder.Sql("EXEC sp_rename N'dbo.FK_CareerPathRules_CareerPaths_CareerPathId', N'FK_CareerMapRules_CareerPaths_CareerPathId';");
            migrationBuilder.Sql("EXEC sp_rename N'dbo.FK_CareerPathRules_Users_ModifiedById', N'FK_CareerMapRules_Users_ModifiedById';");

            migrationBuilder.Sql("EXEC sp_rename N'dbo.PK_CareerPathRuleJobs', N'PK_CareerMapRulePositions';");
            migrationBuilder.Sql("EXEC sp_rename N'dbo.PK_CareerPathRules', N'PK_CareerMapRules';");

            migrationBuilder.RenameIndex(
                name: "IX_CareerPathRules_ModifiedById",
                table: "CareerPathRules",
                newName: "IX_CareerMapRules_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_CareerPathRules_CareerPathId_Grade",
                table: "CareerPathRules",
                newName: "IX_CareerMapRules_CareerPathId_Grade");

            migrationBuilder.RenameIndex(
                name: "IX_CareerPathRuleJobs_JobId",
                table: "CareerPathRuleJobs",
                newName: "IX_CareerMapRulePositions_JobId");

            migrationBuilder.RenameTable(
                name: "CareerPathRules",
                newName: "CareerMapRules");

            migrationBuilder.RenameTable(
                name: "CareerPathRuleJobs",
                newName: "CareerMapRulePositions");

            migrationBuilder.RenameColumn(
                name: "CareerPathRuleId",
                table: "CareerMapRulePositions",
                newName: "CareerMapRuleId");
        }
    }
}
