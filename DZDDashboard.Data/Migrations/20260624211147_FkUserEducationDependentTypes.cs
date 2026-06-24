using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class FkUserEducationDependentTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WorkModel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "UserEducationHistories");

            migrationBuilder.DropColumn(
                name: "DependentType",
                table: "UserBenefitDependents");

            migrationBuilder.DropColumn(
                name: "DependentType",
                table: "ChecklistItemDependents");

            migrationBuilder.AddColumn<int>(
                name: "ContractTypeId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkModelId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EducationLevelId",
                table: "UserEducationHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DependentTypeId",
                table: "UserBenefitDependents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DependentTypeId",
                table: "ChecklistItemDependents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ContractTypeId",
                table: "Users",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_WorkModelId",
                table: "Users",
                column: "WorkModelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEducationHistories_EducationLevelId",
                table: "UserEducationHistories",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefitDependents_DependentTypeId",
                table: "UserBenefitDependents",
                column: "DependentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItemDependents_DependentTypeId",
                table: "ChecklistItemDependents",
                column: "DependentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItemDependents_DependentTypes_DependentTypeId",
                table: "ChecklistItemDependents",
                column: "DependentTypeId",
                principalTable: "DependentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBenefitDependents_DependentTypes_DependentTypeId",
                table: "UserBenefitDependents",
                column: "DependentTypeId",
                principalTable: "DependentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEducationHistories_EducationLevels_EducationLevelId",
                table: "UserEducationHistories",
                column: "EducationLevelId",
                principalTable: "EducationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ContractTypes_ContractTypeId",
                table: "Users",
                column: "ContractTypeId",
                principalTable: "ContractTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_WorkModels_WorkModelId",
                table: "Users",
                column: "WorkModelId",
                principalTable: "WorkModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItemDependents_DependentTypes_DependentTypeId",
                table: "ChecklistItemDependents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBenefitDependents_DependentTypes_DependentTypeId",
                table: "UserBenefitDependents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEducationHistories_EducationLevels_EducationLevelId",
                table: "UserEducationHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ContractTypes_ContractTypeId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_WorkModels_WorkModelId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ContractTypeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_WorkModelId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserEducationHistories_EducationLevelId",
                table: "UserEducationHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserBenefitDependents_DependentTypeId",
                table: "UserBenefitDependents");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistItemDependents_DependentTypeId",
                table: "ChecklistItemDependents");

            migrationBuilder.DropColumn(
                name: "ContractTypeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WorkModelId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EducationLevelId",
                table: "UserEducationHistories");

            migrationBuilder.DropColumn(
                name: "DependentTypeId",
                table: "UserBenefitDependents");

            migrationBuilder.DropColumn(
                name: "DependentTypeId",
                table: "ChecklistItemDependents");

            migrationBuilder.AddColumn<string>(
                name: "ContractType",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkModel",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "UserEducationHistories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DependentType",
                table: "UserBenefitDependents",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DependentType",
                table: "ChecklistItemDependents",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
