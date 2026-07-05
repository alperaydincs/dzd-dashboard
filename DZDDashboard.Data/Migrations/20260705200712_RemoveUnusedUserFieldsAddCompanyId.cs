using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedUserFieldsAddCompanyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Users",
                type: "int",
                nullable: true);

            // Backfill the new FK from the free-text CompanyName before it's dropped.
            migrationBuilder.Sql(@"
                UPDATE u
                SET u.CompanyId = c.Id
                FROM Users u
                JOIN Companies c ON c.Name = u.CompanyName;
            ");

            migrationBuilder.DropColumn(
                name: "ApprovalProcessUnit",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AutoEnrollmentPensionStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CvFilePath",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployeeGroup",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployerPensionEmployeeContribution",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployerPensionEmployerContribution",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployerPensionStartDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HasEmployerPension",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HasPrivateHealthInsurance",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Iban",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MealBenefitAmount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PrivateHealthInsuranceDependentCost",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PrivateHealthInsuranceEmployeeCost",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalProcessUnit",
                table: "Users",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AutoEnrollmentPensionStatus",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Users",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvFilePath",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeGroup",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EmployerPensionEmployeeContribution",
                table: "Users",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EmployerPensionEmployerContribution",
                table: "Users",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmployerPensionStartDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasEmployerPension",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPrivateHealthInsurance",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Iban",
                table: "Users",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MealBenefitAmount",
                table: "Users",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrivateHealthInsuranceDependentCost",
                table: "Users",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrivateHealthInsuranceEmployeeCost",
                table: "Users",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }
    }
}
