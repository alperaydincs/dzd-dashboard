using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCareerMapRuleBenefitFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EmployerContributionUpperLimitAmount",
                table: "CareerMapRules",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployerContributionUpperLimitCurrency",
                table: "CareerMapRules",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MealAllowanceAmount",
                table: "CareerMapRules",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MealAllowanceCurrency",
                table: "CareerMapRules",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrivatePensionInsuranceAmount",
                table: "CareerMapRules",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivatePensionInsuranceCurrency",
                table: "CareerMapRules",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryIncreasePercent",
                table: "CareerMapRules",
                type: "decimal(5,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerContributionUpperLimitAmount",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "EmployerContributionUpperLimitCurrency",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "MealAllowanceAmount",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "MealAllowanceCurrency",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "PrivatePensionInsuranceAmount",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "PrivatePensionInsuranceCurrency",
                table: "CareerMapRules");

            migrationBuilder.DropColumn(
                name: "SalaryIncreasePercent",
                table: "CareerMapRules");
        }
    }
}
