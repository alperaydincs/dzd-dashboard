using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOnboardingOffboardingFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistItemHistory");

            migrationBuilder.DropTable(
                name: "ChecklistItems");

            migrationBuilder.DropTable(
                name: "ChecklistStepTemplateHistory");

            migrationBuilder.DropTable(
                name: "ChecklistStepTemplates");

            migrationBuilder.DropTable(
                name: "DocumentTemplateHistory");

            migrationBuilder.DropTable(
                name: "DocumentTemplates");

            migrationBuilder.DropTable(
                name: "LifecycleAuditLogEntries");

            migrationBuilder.DropTable(
                name: "LifecycleAuditLogEntryHistory");

            migrationBuilder.DropTable(
                name: "OffboardingProcessHistory");

            migrationBuilder.DropTable(
                name: "OnboardingProcessHistory");

            migrationBuilder.DropTable(
                name: "ProcessDocumentHistory");

            migrationBuilder.DropTable(
                name: "ProcessDocuments");

            migrationBuilder.DropTable(
                name: "ProcessTemplateHistory");

            migrationBuilder.DropTable(
                name: "OffboardingProcesses");

            migrationBuilder.DropTable(
                name: "OnboardingProcesses");

            migrationBuilder.DropTable(
                name: "ProcessTemplates");

            migrationBuilder.DropColumn(
                name: "ReviewNote",
                table: "UserCvDocuments");

            migrationBuilder.DropColumn(
                name: "ReviewStatus",
                table: "UserCvDocuments");

            migrationBuilder.DropColumn(
                name: "ReviewNote",
                table: "UserCvDocumentHistory");

            migrationBuilder.DropColumn(
                name: "ReviewStatus",
                table: "UserCvDocumentHistory");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StoredFileHistory");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "UserCvDocuments",
                newName: "UploadedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "UserCvDocumentHistory",
                newName: "UploadedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploadedAt",
                table: "UserCvDocuments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UploadedAt",
                table: "UserCvDocumentHistory",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "ReviewNote",
                table: "UserCvDocuments",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewStatus",
                table: "UserCvDocuments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<string>(
                name: "ReviewNote",
                table: "UserCvDocumentHistory",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewStatus",
                table: "UserCvDocumentHistory",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StoredFileHistory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Files",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ChecklistItemHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedById = table.Column<int>(type: "integer", nullable: true),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    OffboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    OnboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistItemHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistStepTemplateHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ProcessTemplateId = table.Column<int>(type: "integer", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistStepTemplateHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplateHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeadlineDays = table.Column<int>(type: "integer", nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ProcessTemplateId = table.Column<int>(type: "integer", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplateHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "LifecycleAuditLogEntryHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Action = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    OffboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    OnboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PerformedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifecycleAuditLogEntryHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "OffboardingProcessHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TemplateId = table.Column<int>(type: "integer", nullable: false),
                    TemplateName = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffboardingProcessHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingProcessHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ManagerId = table.Column<int>(type: "integer", nullable: true),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TemplateId = table.Column<int>(type: "integer", nullable: false),
                    TemplateName = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingProcessHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessDocumentHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileId = table.Column<int>(type: "integer", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OffboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    OnboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReviewedById = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UploadedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessDocumentHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessTemplateHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Kind = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTemplateHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Kind = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistStepTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProcessTemplateId = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistStepTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistStepTemplates_ProcessTemplates_ProcessTemplateId",
                        column: x => x.ProcessTemplateId,
                        principalTable: "ProcessTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProcessTemplateId = table.Column<int>(type: "integer", nullable: false),
                    DeadlineDays = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTemplates_ProcessTemplates_ProcessTemplateId",
                        column: x => x.ProcessTemplateId,
                        principalTable: "ProcessTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OffboardingProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TemplateId = table.Column<int>(type: "integer", nullable: false),
                    TemplateName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffboardingProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffboardingProcesses_ProcessTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "ProcessTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OffboardingProcesses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ManagerId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TemplateId = table.Column<int>(type: "integer", nullable: false),
                    TemplateName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnboardingProcesses_ProcessTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "ProcessTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnboardingProcesses_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnboardingProcesses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompletedById = table.Column<int>(type: "integer", nullable: true),
                    OffboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    OnboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistItems_OffboardingProcesses_OffboardingProcessId",
                        column: x => x.OffboardingProcessId,
                        principalTable: "OffboardingProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistItems_OnboardingProcesses_OnboardingProcessId",
                        column: x => x.OnboardingProcessId,
                        principalTable: "OnboardingProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistItems_Users_CompletedById",
                        column: x => x.CompletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LifecycleAuditLogEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OffboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    OnboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    PerformedById = table.Column<int>(type: "integer", nullable: true),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Detail = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifecycleAuditLogEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LifecycleAuditLogEntries_OffboardingProcesses_OffboardingPr~",
                        column: x => x.OffboardingProcessId,
                        principalTable: "OffboardingProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LifecycleAuditLogEntries_OnboardingProcesses_OnboardingProc~",
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileId = table.Column<int>(type: "integer", nullable: true),
                    OffboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    OnboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    ReviewedById = table.Column<int>(type: "integer", nullable: true),
                    UploadedById = table.Column<int>(type: "integer", nullable: true),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

            migrationBuilder.InsertData(
                table: "ProcessTemplates",
                columns: new[] { "Id", "Kind", "Name", "Sequence" },
                values: new object[,]
                {
                    { 1, "Onboarding", "General Onboarding", 1 },
                    { 2, "Offboarding", "Resignation", 1 },
                    { 3, "Offboarding", "Termination", 2 }
                });

            migrationBuilder.InsertData(
                table: "ChecklistStepTemplates",
                columns: new[] { "Id", "IsRequired", "ProcessTemplateId", "Sequence", "Title" },
                values: new object[,]
                {
                    { 1, true, 1, 1, "Contract prepared and signed" },
                    { 2, true, 1, 2, "Social security registration completed" },
                    { 3, true, 1, 3, "Accountant notified" },
                    { 4, true, 1, 4, "Private Pension System (BES) account opened" },
                    { 5, true, 1, 5, "Private Health Insurance (ÖSS) opened" },
                    { 6, true, 1, 6, "Computer delivered" },
                    { 7, true, 2, 1, "Resignation letter received" },
                    { 8, true, 2, 2, "Social security exit processed" },
                    { 9, true, 2, 3, "Access revoked" },
                    { 10, true, 2, 4, "Asset return confirmed" },
                    { 11, true, 2, 5, "Final settlement calculated" },
                    { 12, true, 3, 1, "Justification documented" },
                    { 13, true, 3, 2, "Settlement/severance calculated" },
                    { 14, true, 3, 3, "Social security exit processed" },
                    { 15, true, 3, 4, "Access revoked" },
                    { 16, true, 3, 5, "Asset return confirmed" }
                });

            migrationBuilder.InsertData(
                table: "DocumentTemplates",
                columns: new[] { "Id", "DeadlineDays", "IsRequired", "Name", "ProcessTemplateId", "Sequence" },
                values: new object[,]
                {
                    { 1, 4, true, "İkametgâh", 1, 1 },
                    { 2, 4, true, "Diploma", 1, 2 },
                    { 3, 6, true, "Nüfus Kayıt Örneği", 1, 3 },
                    { 4, 9, true, "TC Kimlik Kartı Fotokopisi", 1, 4 },
                    { 5, 9, false, "Adli Sicil Kaydı", 1, 5 },
                    { 6, 9, false, "Akciğer grafisi, hemogram ve göz raporu", 1, 6 },
                    { 7, 7, true, "Akbank Maaş Hesabı Bilgisi", 1, 7 },
                    { 8, 1, true, "İstifa Dilekçesi", 2, 1 },
                    { 9, 7, true, "Zimmet İade Tutanağı", 2, 2 },
                    { 10, 1, true, "Fesih Bildirimi", 3, 1 },
                    { 11, 7, true, "Zimmet İade Tutanağı", 3, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItemHistory_Id",
                table: "ChecklistItemHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_CompletedById",
                table: "ChecklistItems",
                column: "CompletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_OffboardingProcessId",
                table: "ChecklistItems",
                column: "OffboardingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_OnboardingProcessId",
                table: "ChecklistItems",
                column: "OnboardingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistStepTemplateHistory_Id",
                table: "ChecklistStepTemplateHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistStepTemplates_ProcessTemplateId",
                table: "ChecklistStepTemplates",
                column: "ProcessTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplateHistory_Id",
                table: "DocumentTemplateHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_ProcessTemplateId",
                table: "DocumentTemplates",
                column: "ProcessTemplateId");

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
                name: "IX_LifecycleAuditLogEntryHistory_Id",
                table: "LifecycleAuditLogEntryHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OffboardingProcesses_TemplateId",
                table: "OffboardingProcesses",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_OffboardingProcesses_UserId",
                table: "OffboardingProcesses",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OffboardingProcessHistory_Id",
                table: "OffboardingProcessHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingProcesses_ManagerId",
                table: "OnboardingProcesses",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingProcesses_TemplateId",
                table: "OnboardingProcesses",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingProcesses_UserId",
                table: "OnboardingProcesses",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingProcessHistory_Id",
                table: "OnboardingProcessHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDocumentHistory_Id",
                table: "ProcessDocumentHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDocuments_FileId",
                table: "ProcessDocuments",
                column: "FileId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplateHistory_Id",
                table: "ProcessTemplateHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplates_Kind",
                table: "ProcessTemplates",
                column: "Kind");
        }
    }
}
