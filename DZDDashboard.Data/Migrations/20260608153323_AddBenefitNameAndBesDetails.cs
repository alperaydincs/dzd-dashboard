using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    public partial class AddBenefitNameAndBesDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BenefitName",
                table: "UserBenefitRecords",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EmployeeContributionAmount",
                table: "UserBenefitRecords",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EmployerContributionAmount",
                table: "UserBenefitRecords",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolicyNumber",
                table: "UserBenefitRecords",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BenefitName",
                table: "UserBenefitRecords");

            migrationBuilder.DropColumn(
                name: "EmployeeContributionAmount",
                table: "UserBenefitRecords");

            migrationBuilder.DropColumn(
                name: "EmployerContributionAmount",
                table: "UserBenefitRecords");

            migrationBuilder.DropColumn(
                name: "PolicyNumber",
                table: "UserBenefitRecords");
        }
    }
}
