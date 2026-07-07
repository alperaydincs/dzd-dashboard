using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameAuditableEntityToEntityWithHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareerPathRules_Users_ModifiedById",
                table: "CareerPathRules");

            migrationBuilder.DropForeignKey(
                name: "FK_CareerPaths_Users_ModifiedById",
                table: "CareerPaths");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_Users_ModifiedById",
                table: "ChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistStepTemplates_Users_ModifiedById",
                table: "ChecklistStepTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_ModifiedById",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_ModifiedById",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTemplates_Users_ModifiedById",
                table: "DocumentTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_EmergencyContacts_Users_ModifiedById",
                table: "EmergencyContacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_ModifiedById",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Users_ModifiedById",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_OffboardingProcesses_Users_ModifiedById",
                table: "OffboardingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_OnboardingProcesses_Users_ModifiedById",
                table: "OnboardingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationPositions_Users_ModifiedById",
                table: "OrganizationPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollLocations_Users_ModifiedById",
                table: "PayrollLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessDocuments_Users_ModifiedById",
                table: "ProcessDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplates_Users_ModifiedById",
                table: "ProcessTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Users_ModifiedById",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_UdemyCourseActivities_Users_ModifiedById",
                table: "UdemyCourseActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAdditionalPayments_Users_ModifiedById",
                table: "UserAdditionalPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBenefitDependents_Users_ModifiedById",
                table: "UserBenefitDependents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBenefitRecords_Users_ModifiedById",
                table: "UserBenefitRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChildren_Users_ModifiedById",
                table: "UserChildren");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCvDocuments_Users_ModifiedById",
                table: "UserCvDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDeductions_Users_ModifiedById",
                table: "UserDeductions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEducationHistories_Users_ModifiedById",
                table: "UserEducationHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPositionHistories_Users_ModifiedById",
                table: "UserPositionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ModifiedById",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSalaryHistories_Users_ModifiedById",
                table: "UserSalaryHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserSalaryHistories_ModifiedById",
                table: "UserSalaryHistories");

            migrationBuilder.DropIndex(
                name: "IX_Users_ModifiedById",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserPositionHistories_ModifiedById",
                table: "UserPositionHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserEducationHistories_ModifiedById",
                table: "UserEducationHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserDeductions_ModifiedById",
                table: "UserDeductions");

            migrationBuilder.DropIndex(
                name: "IX_UserCvDocuments_ModifiedById",
                table: "UserCvDocuments");

            migrationBuilder.DropIndex(
                name: "IX_UserChildren_ModifiedById",
                table: "UserChildren");

            migrationBuilder.DropIndex(
                name: "IX_UserBenefitRecords_ModifiedById",
                table: "UserBenefitRecords");

            migrationBuilder.DropIndex(
                name: "IX_UserBenefitDependents_ModifiedById",
                table: "UserBenefitDependents");

            migrationBuilder.DropIndex(
                name: "IX_UserAdditionalPayments_ModifiedById",
                table: "UserAdditionalPayments");

            migrationBuilder.DropIndex(
                name: "IX_UdemyCourseActivities_ModifiedById",
                table: "UdemyCourseActivities");

            migrationBuilder.DropIndex(
                name: "IX_Teams_ModifiedById",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTemplates_ModifiedById",
                table: "ProcessTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ProcessDocuments_ModifiedById",
                table: "ProcessDocuments");

            migrationBuilder.DropIndex(
                name: "IX_PayrollLocations_ModifiedById",
                table: "PayrollLocations");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationPositions_ModifiedById",
                table: "OrganizationPositions");

            migrationBuilder.DropIndex(
                name: "IX_OnboardingProcesses_ModifiedById",
                table: "OnboardingProcesses");

            migrationBuilder.DropIndex(
                name: "IX_OffboardingProcesses_ModifiedById",
                table: "OffboardingProcesses");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ModifiedById",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Files_ModifiedById",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_EmergencyContacts_ModifiedById",
                table: "EmergencyContacts");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTemplates_ModifiedById",
                table: "DocumentTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ModifiedById",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ModifiedById",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistStepTemplates_ModifiedById",
                table: "ChecklistStepTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistItems_ModifiedById",
                table: "ChecklistItems");

            migrationBuilder.DropIndex(
                name: "IX_CareerPaths_ModifiedById",
                table: "CareerPaths");

            migrationBuilder.DropIndex(
                name: "IX_CareerPathRules_ModifiedById",
                table: "CareerPathRules");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserSalaryHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserSalaryHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserPositionHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserPositionHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserEducationHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserEducationHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserDeductions");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserDeductions");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserCvDocuments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserCvDocuments");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserCvDocumentHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserCvDocumentHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserChildren");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserChildren");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserBenefitRecords");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserBenefitRecords");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserBenefitDependents");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserBenefitDependents");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserAdditionalPayments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserAdditionalPayments");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UdemyCourseActivityHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UdemyCourseActivityHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UdemyCourseActivities");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UdemyCourseActivities");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "TeamHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "TeamHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "StoredFileHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "StoredFileHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "SalaryHistoryHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "SalaryHistoryHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ProcessTemplates");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ProcessTemplates");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ProcessTemplateHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ProcessTemplateHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ProcessDocuments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ProcessDocuments");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ProcessDocumentHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ProcessDocumentHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "PositionHistoryHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "PositionHistoryHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "PayrollLocations");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "PayrollLocations");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "PayrollLocationHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "PayrollLocationHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "OrganizationPositions");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "OrganizationPositions");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "OrganizationPositionHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "OrganizationPositionHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "OnboardingProcessHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "OnboardingProcessHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "OnboardingProcesses");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "OnboardingProcesses");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "OffboardingProcessHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "OffboardingProcessHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "OffboardingProcesses");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "OffboardingProcesses");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "JobHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "JobHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "EmergencyContacts");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "EmergencyContacts");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "EmergencyContactHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "EmergencyContactHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "EducationHistoryHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "EducationHistoryHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "DocumentTemplates");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "DocumentTemplates");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "DocumentTemplateHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "DocumentTemplateHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "DepartmentHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "DepartmentHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "DeductionHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "DeductionHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "CompanyHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "CompanyHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ChildInfoHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ChildInfoHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ChecklistStepTemplates");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ChecklistStepTemplates");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ChecklistStepTemplateHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ChecklistStepTemplateHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ChecklistItemHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ChecklistItemHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "CareerPaths");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "CareerPaths");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "CareerPathRules");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "CareerPathRules");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "CareerPathRuleHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "CareerPathRuleHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "CareerPathHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "CareerPathHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "BenefitRecordHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "BenefitRecordHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "BenefitDependentHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "BenefitDependentHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "AdditionalPaymentHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "AdditionalPaymentHistory");

            migrationBuilder.CreateTable(
                name: "LifecycleAuditLogEntryHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    OnboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    OffboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: false),
                    PerformedById = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifecycleAuditLogEntryHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LifecycleAuditLogEntryHistory_Id",
                table: "LifecycleAuditLogEntryHistory",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LifecycleAuditLogEntryHistory");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserSalaryHistories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserSalaryHistories",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserPositionHistories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserPositionHistories",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserEducationHistories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserEducationHistories",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserDeductions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserDeductions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserCvDocuments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserCvDocuments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserCvDocumentHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserCvDocumentHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserChildren",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserChildren",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserBenefitRecords",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserBenefitRecords",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserBenefitDependents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserBenefitDependents",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserAdditionalPayments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserAdditionalPayments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UdemyCourseActivityHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UdemyCourseActivityHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UdemyCourseActivities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UdemyCourseActivities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Teams",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Teams",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "TeamHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "TeamHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "StoredFileHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "StoredFileHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "SalaryHistoryHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "SalaryHistoryHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ProcessTemplates",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "ProcessTemplates",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ProcessTemplateHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "ProcessTemplateHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ProcessDocuments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "ProcessDocuments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ProcessDocumentHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "ProcessDocumentHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "PositionHistoryHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "PositionHistoryHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "PayrollLocations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "PayrollLocations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "PayrollLocationHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "PayrollLocationHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "OrganizationPositions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "OrganizationPositions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "OrganizationPositionHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "OrganizationPositionHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "OnboardingProcessHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "OnboardingProcessHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "OnboardingProcesses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "OnboardingProcesses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "OffboardingProcessHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "OffboardingProcessHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "OffboardingProcesses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "OffboardingProcesses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Jobs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Jobs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "JobHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "JobHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Files",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Files",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "EmergencyContacts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "EmergencyContacts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "EmergencyContactHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "EmergencyContactHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "EducationHistoryHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "EducationHistoryHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "DocumentTemplates",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "DocumentTemplates",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "DocumentTemplateHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "DocumentTemplateHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Departments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Departments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "DepartmentHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "DepartmentHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "DeductionHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "DeductionHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "CompanyHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "CompanyHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Companies",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ChildInfoHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "ChildInfoHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ChecklistStepTemplates",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "ChecklistStepTemplates",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ChecklistStepTemplateHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "ChecklistStepTemplateHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ChecklistItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "ChecklistItems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ChecklistItemHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "ChecklistItemHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "CareerPaths",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "CareerPaths",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "CareerPathRules",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "CareerPathRules",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "CareerPathRuleHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "CareerPathRuleHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "CareerPathHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "CareerPathHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "BenefitRecordHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "BenefitRecordHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "BenefitDependentHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "BenefitDependentHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "AdditionalPaymentHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "AdditionalPaymentHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ProcessTemplates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ProcessTemplates",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ProcessTemplates",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ModifiedAt", "ModifiedById" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_UserSalaryHistories_ModifiedById",
                table: "UserSalaryHistories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModifiedById",
                table: "Users",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPositionHistories_ModifiedById",
                table: "UserPositionHistories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserEducationHistories_ModifiedById",
                table: "UserEducationHistories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeductions_ModifiedById",
                table: "UserDeductions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserCvDocuments_ModifiedById",
                table: "UserCvDocuments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserChildren_ModifiedById",
                table: "UserChildren",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefitRecords_ModifiedById",
                table: "UserBenefitRecords",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefitDependents_ModifiedById",
                table: "UserBenefitDependents",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalPayments_ModifiedById",
                table: "UserAdditionalPayments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UdemyCourseActivities_ModifiedById",
                table: "UdemyCourseActivities",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ModifiedById",
                table: "Teams",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplates_ModifiedById",
                table: "ProcessTemplates",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDocuments_ModifiedById",
                table: "ProcessDocuments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLocations_ModifiedById",
                table: "PayrollLocations",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPositions_ModifiedById",
                table: "OrganizationPositions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingProcesses_ModifiedById",
                table: "OnboardingProcesses",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_OffboardingProcesses_ModifiedById",
                table: "OffboardingProcesses",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ModifiedById",
                table: "Jobs",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ModifiedById",
                table: "Files",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_ModifiedById",
                table: "EmergencyContacts",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_ModifiedById",
                table: "DocumentTemplates",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ModifiedById",
                table: "Departments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ModifiedById",
                table: "Companies",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistStepTemplates_ModifiedById",
                table: "ChecklistStepTemplates",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_ModifiedById",
                table: "ChecklistItems",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CareerPaths_ModifiedById",
                table: "CareerPaths",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CareerPathRules_ModifiedById",
                table: "CareerPathRules",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CareerPathRules_Users_ModifiedById",
                table: "CareerPathRules",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CareerPaths_Users_ModifiedById",
                table: "CareerPaths",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_Users_ModifiedById",
                table: "ChecklistItems",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistStepTemplates_Users_ModifiedById",
                table: "ChecklistStepTemplates",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_ModifiedById",
                table: "Companies",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_ModifiedById",
                table: "Departments",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTemplates_Users_ModifiedById",
                table: "DocumentTemplates",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmergencyContacts_Users_ModifiedById",
                table: "EmergencyContacts",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_ModifiedById",
                table: "Files",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Users_ModifiedById",
                table: "Jobs",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OffboardingProcesses_Users_ModifiedById",
                table: "OffboardingProcesses",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OnboardingProcesses_Users_ModifiedById",
                table: "OnboardingProcesses",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationPositions_Users_ModifiedById",
                table: "OrganizationPositions",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollLocations_Users_ModifiedById",
                table: "PayrollLocations",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDocuments_Users_ModifiedById",
                table: "ProcessDocuments",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplates_Users_ModifiedById",
                table: "ProcessTemplates",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Users_ModifiedById",
                table: "Teams",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UdemyCourseActivities_Users_ModifiedById",
                table: "UdemyCourseActivities",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAdditionalPayments_Users_ModifiedById",
                table: "UserAdditionalPayments",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBenefitDependents_Users_ModifiedById",
                table: "UserBenefitDependents",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBenefitRecords_Users_ModifiedById",
                table: "UserBenefitRecords",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChildren_Users_ModifiedById",
                table: "UserChildren",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCvDocuments_Users_ModifiedById",
                table: "UserCvDocuments",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeductions_Users_ModifiedById",
                table: "UserDeductions",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEducationHistories_Users_ModifiedById",
                table: "UserEducationHistories",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositionHistories_Users_ModifiedById",
                table: "UserPositionHistories",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ModifiedById",
                table: "Users",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSalaryHistories_Users_ModifiedById",
                table: "UserSalaryHistories",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
