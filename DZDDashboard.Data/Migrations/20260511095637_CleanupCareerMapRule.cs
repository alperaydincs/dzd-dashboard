using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class CleanupCareerMapRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerContributionLimit",
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

            migrationBuilder.RenameColumn(
                name: "MinRoleTimeyear",
                table: "CareerMapRules",
                newName: "MinRoleTimeYear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinRoleTimeYear",
                table: "CareerMapRules",
                newName: "MinRoleTimeyear");

            migrationBuilder.AddColumn<string>(
                name: "EmployerContributionLimit",
                table: "CareerMapRules",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

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
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tenure",
                table: "CareerMapRules",
                type: "int",
                nullable: true);
        }
    }
}
