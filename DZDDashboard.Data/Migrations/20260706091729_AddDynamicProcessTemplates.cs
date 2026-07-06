using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDynamicProcessTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DocumentTemplates_ProcessType",
                table: "DocumentTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistStepTemplates_ProcessType",
                table: "ChecklistStepTemplates");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "OffboardingProcesses");

            migrationBuilder.DropColumn(
                name: "ProcessType",
                table: "DocumentTemplates");

            migrationBuilder.DropColumn(
                name: "ProcessType",
                table: "ChecklistStepTemplates");

            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "OnboardingProcesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "OnboardingProcesses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "OffboardingProcesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "OffboardingProcesses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProcessTemplateId",
                table: "DocumentTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProcessTemplateId",
                table: "ChecklistStepTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProcessTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kind = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessTemplates_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 7,
                column: "ProcessTemplateId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 8,
                column: "ProcessTemplateId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 9,
                column: "ProcessTemplateId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 10,
                column: "ProcessTemplateId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 11,
                column: "ProcessTemplateId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 12,
                column: "ProcessTemplateId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 13,
                column: "ProcessTemplateId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 14,
                column: "ProcessTemplateId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 15,
                column: "ProcessTemplateId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 16,
                column: "ProcessTemplateId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 7,
                column: "ProcessTemplateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 8,
                column: "ProcessTemplateId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 9,
                column: "ProcessTemplateId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 10,
                column: "ProcessTemplateId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 11,
                column: "ProcessTemplateId",
                value: 3);

            migrationBuilder.InsertData(
                table: "ProcessTemplates",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Kind", "ModifiedAt", "ModifiedById", "Name", "Sequence" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Onboarding", null, null, "General Onboarding", 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Offboarding", null, null, "Resignation", 1 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Offboarding", null, null, "Termination", 2 }
                });

            // Any rows created before this migration (e.g. templates added via the app while this feature
            // was in development) default to ProcessTemplateId/TemplateId 0; point them at a real template
            // so the new foreign keys below don't fail.
            migrationBuilder.Sql("UPDATE [ChecklistStepTemplates] SET [ProcessTemplateId] = 1 WHERE [ProcessTemplateId] NOT IN (SELECT [Id] FROM [ProcessTemplates])");
            migrationBuilder.Sql("UPDATE [DocumentTemplates] SET [ProcessTemplateId] = 1 WHERE [ProcessTemplateId] NOT IN (SELECT [Id] FROM [ProcessTemplates])");
            migrationBuilder.Sql("UPDATE [OnboardingProcesses] SET [TemplateId] = 1, [TemplateName] = 'General Onboarding' WHERE [TemplateId] NOT IN (SELECT [Id] FROM [ProcessTemplates])");
            migrationBuilder.Sql("UPDATE [OffboardingProcesses] SET [TemplateId] = 2, [TemplateName] = 'Resignation' WHERE [TemplateId] NOT IN (SELECT [Id] FROM [ProcessTemplates])");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingProcesses_TemplateId",
                table: "OnboardingProcesses",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_OffboardingProcesses_TemplateId",
                table: "OffboardingProcesses",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_ProcessTemplateId",
                table: "DocumentTemplates",
                column: "ProcessTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistStepTemplates_ProcessTemplateId",
                table: "ChecklistStepTemplates",
                column: "ProcessTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplates_Kind",
                table: "ProcessTemplates",
                column: "Kind");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplates_ModifiedById",
                table: "ProcessTemplates",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistStepTemplates_ProcessTemplates_ProcessTemplateId",
                table: "ChecklistStepTemplates",
                column: "ProcessTemplateId",
                principalTable: "ProcessTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTemplates_ProcessTemplates_ProcessTemplateId",
                table: "DocumentTemplates",
                column: "ProcessTemplateId",
                principalTable: "ProcessTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OffboardingProcesses_ProcessTemplates_TemplateId",
                table: "OffboardingProcesses",
                column: "TemplateId",
                principalTable: "ProcessTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OnboardingProcesses_ProcessTemplates_TemplateId",
                table: "OnboardingProcesses",
                column: "TemplateId",
                principalTable: "ProcessTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistStepTemplates_ProcessTemplates_ProcessTemplateId",
                table: "ChecklistStepTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTemplates_ProcessTemplates_ProcessTemplateId",
                table: "DocumentTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_OffboardingProcesses_ProcessTemplates_TemplateId",
                table: "OffboardingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_OnboardingProcesses_ProcessTemplates_TemplateId",
                table: "OnboardingProcesses");

            migrationBuilder.DropTable(
                name: "ProcessTemplates");

            migrationBuilder.DropIndex(
                name: "IX_OnboardingProcesses_TemplateId",
                table: "OnboardingProcesses");

            migrationBuilder.DropIndex(
                name: "IX_OffboardingProcesses_TemplateId",
                table: "OffboardingProcesses");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTemplates_ProcessTemplateId",
                table: "DocumentTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistStepTemplates_ProcessTemplateId",
                table: "ChecklistStepTemplates");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "OnboardingProcesses");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "OnboardingProcesses");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "OffboardingProcesses");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "OffboardingProcesses");

            migrationBuilder.DropColumn(
                name: "ProcessTemplateId",
                table: "DocumentTemplates");

            migrationBuilder.DropColumn(
                name: "ProcessTemplateId",
                table: "ChecklistStepTemplates");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "OffboardingProcesses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProcessType",
                table: "DocumentTemplates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProcessType",
                table: "ChecklistStepTemplates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 7,
                column: "ProcessType",
                value: "Offboarding:Resignation");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 8,
                column: "ProcessType",
                value: "Offboarding:Resignation");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 9,
                column: "ProcessType",
                value: "Offboarding:Resignation");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 10,
                column: "ProcessType",
                value: "Offboarding:Resignation");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 11,
                column: "ProcessType",
                value: "Offboarding:Resignation");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 12,
                column: "ProcessType",
                value: "Offboarding:Termination");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 13,
                column: "ProcessType",
                value: "Offboarding:Termination");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 14,
                column: "ProcessType",
                value: "Offboarding:Termination");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 15,
                column: "ProcessType",
                value: "Offboarding:Termination");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 16,
                column: "ProcessType",
                value: "Offboarding:Termination");

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 7,
                column: "ProcessType",
                value: "Onboarding");

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 8,
                column: "ProcessType",
                value: "Offboarding:Resignation");

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 9,
                column: "ProcessType",
                value: "Offboarding:Resignation");

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 10,
                column: "ProcessType",
                value: "Offboarding:Termination");

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 11,
                column: "ProcessType",
                value: "Offboarding:Termination");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_ProcessType",
                table: "DocumentTemplates",
                column: "ProcessType");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistStepTemplates_ProcessType",
                table: "ChecklistStepTemplates",
                column: "ProcessType");
        }
    }
}
