using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class ParametricChecklistTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_StoredFiles_EvidenceStoredFileId",
                table: "ChecklistItems");

            migrationBuilder.RenameColumn(
                name: "EvidenceStoredFileId",
                table: "ChecklistItems",
                newName: "DocumentStoredFileId");

            migrationBuilder.RenameColumn(
                name: "EvidenceFileName",
                table: "ChecklistItems",
                newName: "DocumentFileName");

            migrationBuilder.RenameColumn(
                name: "EvidenceContentType",
                table: "ChecklistItems",
                newName: "DocumentContentType");

            migrationBuilder.RenameIndex(
                name: "IX_ChecklistItems_EvidenceStoredFileId",
                table: "ChecklistItems",
                newName: "IX_ChecklistItems_DocumentStoredFileId");

            migrationBuilder.AddColumn<bool>(
                name: "RequiresDocument",
                table: "ChecklistItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ChecklistStepTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StepKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsGate = table.Column<bool>(type: "bit", nullable: false),
                    RequiresDocument = table.Column<bool>(type: "bit", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    BenefitKind = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistStepTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistStepTemplates_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ChecklistStepTemplates",
                columns: new[] { "Id", "BenefitKind", "CreatedAt", "IsEnabled", "IsGate", "IsRequired", "ModifiedAt", "ModifiedById", "ProcessType", "RequiresDocument", "Sequence", "StepKey", "Title" },
                values: new object[,]
                {
                    { 1, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Onboarding", false, 1, "documents", "Zorunlu evraklar tamamlandı" },
                    { 2, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Onboarding", false, 2, "contract", "Sözleşme hazırlandı ve imzalatıldı" },
                    { 3, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Onboarding", false, 3, "sgk-entry", "SGK işe giriş yapıldı" },
                    { 4, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Onboarding", false, 4, "accountant-info", "Mali müşavire bilgi verildi" },
                    { 5, "BES", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Onboarding", false, 5, "bes", "BES açıldı (işveren + çalışan payı)" },
                    { 6, "OSS", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Onboarding", false, 6, "oss", "ÖSS açıldı (çalışan + bağımlılar)" },
                    { 7, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Onboarding", false, 7, "asset-handover", "Bilgisayar teslim edildi (zimmet)" },
                    { 8, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Resignation", false, 1, "resignation-letter", "İstifa alındı" },
                    { 9, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Resignation", false, 2, "sgk-exit", "SGK işten çıkış" },
                    { 10, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Resignation", false, 3, "oss-cancel", "ÖSS iptal" },
                    { 11, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Resignation", false, 4, "bes-cancel", "BES iptal" },
                    { 12, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Resignation", false, 5, "access-revocation", "Erişim kapatma" },
                    { 13, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Resignation", false, 6, "asset-return", "Zimmet iadesi" },
                    { 14, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Resignation", false, 7, "final-settlement", "Hakediş hesaplama" },
                    { 15, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, true, null, null, "Offboarding:Resignation", false, 8, "payment-done", "Ödeme yapıldı" },
                    { 16, "None", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, false, true, null, null, "Offboarding:Termination", false, 1, "justification", "Gerekçe dokümantasyonu" },
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
                name: "IX_ChecklistStepTemplates_ModifiedById",
                table: "ChecklistStepTemplates",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistStepTemplates_ProcessType_StepKey",
                table: "ChecklistStepTemplates",
                columns: new[] { "ProcessType", "StepKey" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_StoredFiles_DocumentStoredFileId",
                table: "ChecklistItems",
                column: "DocumentStoredFileId",
                principalTable: "StoredFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_StoredFiles_DocumentStoredFileId",
                table: "ChecklistItems");

            migrationBuilder.DropTable(
                name: "ChecklistStepTemplates");

            migrationBuilder.DropColumn(
                name: "RequiresDocument",
                table: "ChecklistItems");

            migrationBuilder.RenameColumn(
                name: "DocumentStoredFileId",
                table: "ChecklistItems",
                newName: "EvidenceStoredFileId");

            migrationBuilder.RenameColumn(
                name: "DocumentFileName",
                table: "ChecklistItems",
                newName: "EvidenceFileName");

            migrationBuilder.RenameColumn(
                name: "DocumentContentType",
                table: "ChecklistItems",
                newName: "EvidenceContentType");

            migrationBuilder.RenameIndex(
                name: "IX_ChecklistItems_DocumentStoredFileId",
                table: "ChecklistItems",
                newName: "IX_ChecklistItems_EvidenceStoredFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_StoredFiles_EvidenceStoredFileId",
                table: "ChecklistItems",
                column: "EvidenceStoredFileId",
                principalTable: "StoredFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
