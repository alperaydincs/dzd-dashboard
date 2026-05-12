using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCareerMapExtensions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployerContributionLimit",
                table: "CareerMapRules",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Evaluation",
                table: "CareerMapRules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceMaxYear",
                table: "CareerMapRules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceMinYear",
                table: "CareerMapRules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MealAllowance",
                table: "CareerMapRules",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivatePensionInsurance",
                table: "CareerMapRules",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryIncreasePercent",
                table: "CareerMapRules",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tenure",
                table: "CareerMapRules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CareerMapRulePositions",
                columns: table => new
                {
                    CareerMapRuleId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerMapRulePositions", x => new { x.CareerMapRuleId, x.JobId });
                    table.ForeignKey(
                        name: "FK_CareerMapRulePositions_CareerMapRules_CareerMapRuleId",
                        column: x => x.CareerMapRuleId,
                        principalTable: "CareerMapRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CareerMapRulePositions_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareerMapRulePositions_JobId",
                table: "CareerMapRulePositions",
                column: "JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CareerMapRulePositions");

            migrationBuilder.DropColumn(
                name: "EmployerContributionLimit",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "Evaluation",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "ExperienceMaxYear",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "ExperienceMinYear",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "MealAllowance",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "PrivatePensionInsurance",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "SalaryIncreasePercent",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "Tenure",
                table: "CareerMapRules");
        }
    }
}
