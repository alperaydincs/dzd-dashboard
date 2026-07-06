using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RewriteOnboardingOffboarding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistItemDependents");

            migrationBuilder.DropTable(
                name: "UserOffboardingDocuments");

            migrationBuilder.DropTable(
                name: "UserOnboardingDocuments");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistStepTemplates_ProcessType_StepKey",
                table: "ChecklistStepTemplates");

            migrationBuilder.DeleteData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DropColumn(
                name: "BenefitKind",
                table: "ChecklistStepTemplates");

            migrationBuilder.DropColumn(
                name: "IsGate",
                table: "ChecklistStepTemplates");

            migrationBuilder.DropColumn(
                name: "RequiresDocument",
                table: "ChecklistStepTemplates");

            migrationBuilder.DropColumn(
                name: "StepKey",
                table: "ChecklistStepTemplates");

            migrationBuilder.DropColumn(
                name: "BenefitKind",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "EmployeeAmount",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "EmployerAmount",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "IsGate",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "ProviderName",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "ReflectedBenefitRecordId",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "RequiresDocument",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "StepKey",
                table: "ChecklistItems");

            migrationBuilder.CreateTable(
                name: "DocumentTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    DeadlineDays = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTemplates_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LifecycleAuditLogEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OnboardingProcessId = table.Column<int>(type: "int", nullable: true),
                    OffboardingProcessId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PerformedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifecycleAuditLogEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LifecycleAuditLogEntries_OffboardingProcesses_OffboardingProcessId",
                        column: x => x.OffboardingProcessId,
                        principalTable: "OffboardingProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LifecycleAuditLogEntries_OnboardingProcesses_OnboardingProcessId",
                        column: x => x.OnboardingProcessId,
                        principalTable: "OnboardingProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LifecycleAuditLogEntries_Users_PerformedById",
                        column: x => x.PerformedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OnboardingProcessId = table.Column<int>(type: "int", nullable: true),
                    OffboardingProcessId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileId = table.Column<int>(type: "int", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UploadedById = table.Column<int>(type: "int", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessDocuments_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessDocuments_OffboardingProcesses_OffboardingProcessId",
                        column: x => x.OffboardingProcessId,
                        principalTable: "OffboardingProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessDocuments_OnboardingProcesses_OnboardingProcessId",
                        column: x => x.OnboardingProcessId,
                        principalTable: "OnboardingProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessDocuments_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessDocuments_Users_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessDocuments_Users_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title",
                value: "Contract prepared and signed");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "Social security registration completed");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title",
                value: "Accountant notified");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 4,
                column: "Title",
                value: "Private Pension System (BES) account opened");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 5,
                column: "Title",
                value: "Private Health Insurance (ÖSS) opened");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 6,
                column: "Title",
                value: "Computer delivered");

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ProcessType", "Sequence", "Title" },
                values: new object[] { "Offboarding:Resignation", 1, "Resignation letter received" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Sequence", "Title" },
                values: new object[] { 2, "Social security exit processed" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Sequence", "Title" },
                values: new object[] { 3, "Access revoked" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Sequence", "Title" },
                values: new object[] { 4, "Asset return confirmed" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Sequence", "Title" },
                values: new object[] { 5, "Final settlement calculated" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ProcessType", "Sequence", "Title" },
                values: new object[] { "Offboarding:Termination", 1, "Justification documented" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ProcessType", "Sequence", "Title" },
                values: new object[] { "Offboarding:Termination", 2, "Settlement/severance calculated" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ProcessType", "Sequence", "Title" },
                values: new object[] { "Offboarding:Termination", 3, "Social security exit processed" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ProcessType", "Sequence", "Title" },
                values: new object[] { "Offboarding:Termination", 4, "Access revoked" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Sequence", "Title" },
                values: new object[] { 5, "Asset return confirmed" });

            migrationBuilder.InsertData(
                table: "DocumentTemplates",
                columns: new[] { "Id", "CreatedAt", "DeadlineDays", "IsEnabled", "IsRequired", "ModifiedAt", "ModifiedById", "Name", "ProcessType", "Sequence" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, true, true, null, null, "İkametgâh", "Onboarding", 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, true, true, null, null, "Diploma", "Onboarding", 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, true, true, null, null, "Nüfus Kayıt Örneği", "Onboarding", 3 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, true, true, null, null, "TC Kimlik Kartı Fotokopisi", "Onboarding", 4 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, true, false, null, null, "Adli Sicil Kaydı", "Onboarding", 5 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, true, false, null, null, "Akciğer grafisi, hemogram ve göz raporu", "Onboarding", 6 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, true, true, null, null, "Akbank Maaş Hesabı Bilgisi", "Onboarding", 7 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, true, null, null, "İstifa Dilekçesi", "Offboarding:Resignation", 1 },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, true, true, null, null, "Zimmet İade Tutanağı", "Offboarding:Resignation", 2 },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, true, null, null, "Fesih Bildirimi", "Offboarding:Termination", 1 },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, true, true, null, null, "Zimmet İade Tutanağı", "Offboarding:Termination", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistStepTemplates_ProcessType",
                table: "ChecklistStepTemplates",
                column: "ProcessType");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_ModifiedById",
                table: "DocumentTemplates",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_ProcessType",
                table: "DocumentTemplates",
                column: "ProcessType");

            migrationBuilder.CreateIndex(
                name: "IX_LifecycleAuditLogEntries_OffboardingProcessId",
                table: "LifecycleAuditLogEntries",
                column: "OffboardingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_LifecycleAuditLogEntries_OnboardingProcessId",
                table: "LifecycleAuditLogEntries",
                column: "OnboardingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_LifecycleAuditLogEntries_PerformedById",
                table: "LifecycleAuditLogEntries",
                column: "PerformedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDocuments_FileId",
                table: "ProcessDocuments",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDocuments_ModifiedById",
                table: "ProcessDocuments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDocuments_OffboardingProcessId",
                table: "ProcessDocuments",
                column: "OffboardingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDocuments_OnboardingProcessId",
                table: "ProcessDocuments",
                column: "OnboardingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDocuments_ReviewedById",
                table: "ProcessDocuments",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDocuments_UploadedById",
                table: "ProcessDocuments",
                column: "UploadedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentTemplates");

            migrationBuilder.DropTable(
                name: "LifecycleAuditLogEntries");

            migrationBuilder.DropTable(
                name: "ProcessDocuments");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistStepTemplates_ProcessType",
                table: "ChecklistStepTemplates");

            migrationBuilder.AddColumn<string>(
                name: "BenefitKind",
                table: "ChecklistStepTemplates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsGate",
                table: "ChecklistStepTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresDocument",
                table: "ChecklistStepTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StepKey",
                table: "ChecklistStepTemplates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BenefitKind",
                table: "ChecklistItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "ChecklistItems",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EmployeeAmount",
                table: "ChecklistItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EmployerAmount",
                table: "ChecklistItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGate",
                table: "ChecklistItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ChecklistItems",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderName",
                table: "ChecklistItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReflectedBenefitRecordId",
                table: "ChecklistItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresDocument",
                table: "ChecklistItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StepKey",
                table: "ChecklistItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ChecklistItemDependents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistItemId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DependentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    RelationType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistItemDependents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistItemDependents_ChecklistItems_ChecklistItemId",
                        column: x => x.ChecklistItemId,
                        principalTable: "ChecklistItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOffboardingDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistItemId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOffboardingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOffboardingDocuments_ChecklistItems_ChecklistItemId",
                        column: x => x.ChecklistItemId,
                        principalTable: "ChecklistItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOffboardingDocuments_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserOffboardingDocuments_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserOnboardingDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistItemId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOnboardingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOnboardingDocuments_ChecklistItems_ChecklistItemId",
                        column: x => x.ChecklistItemId,
                        principalTable: "ChecklistItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOnboardingDocuments_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserOnboardingDocuments_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BenefitKind", "IsGate", "RequiresDocument", "StepKey", "Title" },
                values: new object[] { "None", false, false, "documents", "Zorunlu evraklar tamamlandı" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BenefitKind", "IsGate", "RequiresDocument", "StepKey", "Title" },
                values: new object[] { "None", false, false, "contract", "Sözleşme hazırlandı ve imzalatıldı" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BenefitKind", "IsGate", "RequiresDocument", "StepKey", "Title" },
                values: new object[] { "None", false, false, "sgk-entry", "SGK işe giriş yapıldı" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BenefitKind", "IsGate", "RequiresDocument", "StepKey", "Title" },
                values: new object[] { "None", false, false, "accountant-info", "Mali müşavire bilgi verildi" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BenefitKind", "IsGate", "RequiresDocument", "StepKey", "Title" },
                values: new object[] { "BES", false, false, "bes", "BES açıldı (işveren + çalışan payı)" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BenefitKind", "IsGate", "RequiresDocument", "StepKey", "Title" },
                values: new object[] { "OSS", false, false, "oss", "ÖSS açıldı (çalışan + bağımlılar)" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "BenefitKind", "IsGate", "ProcessType", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[] { "None", false, "Onboarding", false, 7, "asset-handover", "Bilgisayar teslim edildi (zimmet)" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "BenefitKind", "IsGate", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[] { "None", false, false, 1, "resignation-letter", "İstifa alındı" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "BenefitKind", "IsGate", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[] { "None", false, false, 2, "sgk-exit", "SGK işten çıkış" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "BenefitKind", "IsGate", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[] { "None", false, false, 3, "oss-cancel", "ÖSS iptal" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "BenefitKind", "IsGate", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[] { "None", false, false, 4, "bes-cancel", "BES iptal" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "BenefitKind", "IsGate", "ProcessType", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[] { "None", false, "Offboarding:Resignation", false, 5, "access-revocation", "Erişim kapatma" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "BenefitKind", "IsGate", "ProcessType", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[] { "None", false, "Offboarding:Resignation", false, 6, "asset-return", "Zimmet iadesi" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "BenefitKind", "IsGate", "ProcessType", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[] { "None", false, "Offboarding:Resignation", false, 7, "final-settlement", "Hakediş hesaplama" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "BenefitKind", "IsGate", "ProcessType", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[] { "None", true, "Offboarding:Resignation", false, 8, "payment-done", "Ödeme yapıldı" });

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "BenefitKind", "IsGate", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[] { "None", false, false, 1, "justification", "Gerekçe dokümantasyonu" });

            migrationBuilder.InsertData(
                table: "ChecklistStepTemplates",
                columns: new[] { "Id", "BenefitKind", "CreatedAt", "IsEnabled", "IsGate", "IsRequired", "ModifiedAt", "ModifiedById", "ProcessType", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[,]
                {
                    { 17, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, true, null, null, "Offboarding:Termination", false, 2, "mediator-meeting", "Arabulucu/avukat görüşmesi (zorunlu)" },
                    { 18, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Termination", false, 3, "settlement-calc", "Hakediş/tazminat hesaplama" },
                    { 19, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, false, null, null, "Offboarding:Termination", false, 4, "approval", "Onay" },
                    { 20, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Termination", false, 5, "sgk-exit", "SGK işten çıkış" },
                    { 21, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Termination", false, 6, "oss-bes-cancel", "ÖSS/BES iptal" },
                    { 22, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Termination", false, 7, "access-revocation", "Erişim kapatma" },
                    { 23, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Termination", false, 8, "asset-return", "Zimmet iadesi" },
                    { 24, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, true, null, null, "Offboarding:Termination", false, 9, "payment-done", "Ödeme yapıldı" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistStepTemplates_ProcessType_StepKey",
                table: "ChecklistStepTemplates",
                columns: new[] { "ProcessType", "StepKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItemDependents_ChecklistItemId",
                table: "ChecklistItemDependents",
                column: "ChecklistItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOffboardingDocuments_ChecklistItemId",
                table: "UserOffboardingDocuments",
                column: "ChecklistItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserOffboardingDocuments_FileId",
                table: "UserOffboardingDocuments",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOffboardingDocuments_ModifiedById",
                table: "UserOffboardingDocuments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserOnboardingDocuments_ChecklistItemId",
                table: "UserOnboardingDocuments",
                column: "ChecklistItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserOnboardingDocuments_FileId",
                table: "UserOnboardingDocuments",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOnboardingDocuments_ModifiedById",
                table: "UserOnboardingDocuments",
                column: "ModifiedById");
        }
    }
}
