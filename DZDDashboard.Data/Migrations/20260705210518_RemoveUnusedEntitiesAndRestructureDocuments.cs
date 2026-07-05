using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedEntitiesAndRestructureDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_StoredFiles_DocumentStoredFileId",
                table: "ChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StoredFiles_Users_ModifiedById",
                table: "StoredFiles");

            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "Users",
                type: "int",
                nullable: true);

            // Backfill the new direct FK from the old UserAvatars wrapper table before it's dropped.
            migrationBuilder.Sql(@"
                UPDATE u
                SET u.AvatarId = ua.StorageId
                FROM Users u
                JOIN UserAvatars ua ON ua.UserId = u.Id;
            ");

            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "DefaultDocuments");

            migrationBuilder.DropTable(
                name: "ExCompanyHistories");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "HeadLeadCoefficients");

            migrationBuilder.DropTable(
                name: "Itsms");

            migrationBuilder.DropTable(
                name: "ProjectBonusCoefficients");

            migrationBuilder.DropTable(
                name: "ProjectDocuments");

            migrationBuilder.DropTable(
                name: "ProjectInvoices");

            migrationBuilder.DropTable(
                name: "TargetEfforts");

            migrationBuilder.DropTable(
                name: "UserAvatars");

            migrationBuilder.DropTable(
                name: "UserGradeHistories");

            migrationBuilder.DropTable(
                name: "UserTrainings");

            migrationBuilder.DropTable(
                name: "IssuePaymentTypes");

            migrationBuilder.DropTable(
                name: "IssuePriorities");

            migrationBuilder.DropTable(
                name: "IssueStatuses");

            migrationBuilder.DropTable(
                name: "IssueTypes");

            migrationBuilder.DropTable(
                name: "Resolutions");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Trainings");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "DzdStatuses");

            migrationBuilder.DropTable(
                name: "JiraStatuses");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "Salesforces");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistItems_DocumentStoredFileId",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "DocumentContentType",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "DocumentFileName",
                table: "ChecklistItems");

            migrationBuilder.DropColumn(
                name: "DocumentStoredFileId",
                table: "ChecklistItems");

            migrationBuilder.RenameTable(
                name: "StoredFiles",
                newName: "Files");

            migrationBuilder.RenameIndex(
                name: "IX_StoredFiles_ModifiedById",
                table: "Files",
                newName: "IX_Files_ModifiedById");

            migrationBuilder.CreateTable(
                name: "UserCvDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FileId = table.Column<int>(type: "int", nullable: true),
                    ReviewStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    ReviewNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCvDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCvDocuments_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCvDocuments_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCvDocuments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Carry over existing CV documents from the old single-table UserDocuments before dropping it.
            migrationBuilder.Sql(@"
                INSERT INTO UserCvDocuments (UserId, FileName, ContentType, SizeBytes, IsActive, FileId, ReviewStatus, ReviewNote, CreatedAt, ModifiedAt, ModifiedById)
                SELECT UserId, FileName, ContentType, SizeBytes, IsActive, StoredFileId, ReviewStatus, ReviewNote, CreatedAt, ModifiedAt, ModifiedById
                FROM UserDocuments;
            ");

            migrationBuilder.DropTable(
                name: "UserDocuments");

            migrationBuilder.DropTable(
                name: "UserDocumentCategories");

            migrationBuilder.CreateTable(
                name: "UserOffboardingDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistItemId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarId",
                table: "Users",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCvDocuments_FileId",
                table: "UserCvDocuments",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCvDocuments_ModifiedById",
                table: "UserCvDocuments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserCvDocuments_UserId_FileName",
                table: "UserCvDocuments",
                columns: new[] { "UserId", "FileName" },
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_ModifiedById",
                table: "Files",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_AvatarId",
                table: "Users",
                column: "AvatarId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_ModifiedById",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_AvatarId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserCvDocuments");

            migrationBuilder.DropTable(
                name: "UserOffboardingDocuments");

            migrationBuilder.DropTable(
                name: "UserOnboardingDocuments");

            migrationBuilder.DropIndex(
                name: "IX_Users_AvatarId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "StoredFiles");

            migrationBuilder.RenameIndex(
                name: "IX_Files_ModifiedById",
                table: "StoredFiles",
                newName: "IX_StoredFiles_ModifiedById");

            migrationBuilder.AddColumn<string>(
                name: "DocumentContentType",
                table: "ChecklistItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentFileName",
                table: "ChecklistItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentStoredFileId",
                table: "ChecklistItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banks_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DefaultDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefaultDocuments_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DzdStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DzdStatusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DzdStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DzdStatuses_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExCompanyHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExCompanyHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExCompanyHistories_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExCompanyHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NextStepId = table.Column<int>(type: "int", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaxSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Grades_NextStepId",
                        column: x => x.NextStepId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssuePriorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PriorityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuePriorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuePriorities_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssueStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueStatuses_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssueTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueTypes_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JiraStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JiraStatusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JiraStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JiraStatuses_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PeriodName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Periods_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resolutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolutionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resolutions_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Salesforces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    PayrollLocationId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsSuitable = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaskPo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TaskTeam = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salesforces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salesforces_PayrollLocations_PayrollLocationId",
                        column: x => x.PayrollLocationId,
                        principalTable: "PayrollLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Salesforces_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InstructorCompanyDetails = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    InstructorName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RepeatFrequency = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainings_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAvatars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    StorageId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvatars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAvatars_StoredFiles_StorageId",
                        column: x => x.StorageId,
                        principalTable: "StoredFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAvatars_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAvatars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDocumentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocumentCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDocumentCategories_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGradeHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGradeHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGradeHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HeadLeadCoefficients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    Coefficient = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadLeadCoefficients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeadLeadCoefficients_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeadLeadCoefficients_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HeadLeadCoefficients_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssuePaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    Coefficient = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentTypeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuePaymentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuePaymentTypes_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_IssuePaymentTypes_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBonusCoefficients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    Coefficient = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBonusCoefficients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectBonusCoefficients_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectBonusCoefficients_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    DzdStatusId = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    PayrollLocationId = table.Column<int>(type: "int", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EFaturaNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JiraProjectNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JiraTaskNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PartialInvoice = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PurchaseInvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatIncludedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectInvoices_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProjectInvoices_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProjectInvoices_DzdStatuses_DzdStatusId",
                        column: x => x.DzdStatusId,
                        principalTable: "DzdStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProjectInvoices_PayrollLocations_PayrollLocationId",
                        column: x => x.PayrollLocationId,
                        principalTable: "PayrollLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProjectInvoices_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProjectInvoices_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TargetEfforts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CompletedTarget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ItsmBonusAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ManagerBonusAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ManagerBonusEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectBonusAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingTarget = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "[Target] - [CompletedTarget]", stored: true),
                    Target = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBonusAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetEfforts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetEfforts_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TargetEfforts_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TargetEfforts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalystId = table.Column<int>(type: "int", nullable: true),
                    BankId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    DeveloperId = table.Column<int>(type: "int", nullable: true),
                    DzdStatusId = table.Column<int>(type: "int", nullable: true),
                    IntertechTeamId = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    ProjectManagerId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    AnalysisDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JiraProjectNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TshirtSize = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UatDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bids_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bids_DzdStatuses_DzdStatusId",
                        column: x => x.DzdStatusId,
                        principalTable: "DzdStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bids_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bids_Salesforces_IntertechTeamId",
                        column: x => x.IntertechTeamId,
                        principalTable: "Salesforces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bids_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bids_Users_AnalystId",
                        column: x => x.AnalystId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bids_Users_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bids_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bids_Users_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalystId = table.Column<int>(type: "int", nullable: true),
                    BankId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    DeveloperId = table.Column<int>(type: "int", nullable: true),
                    DzdStatusId = table.Column<int>(type: "int", nullable: true),
                    IntertechTeamId = table.Column<int>(type: "int", nullable: true),
                    JiraStatusId = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    ProjectManagerId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    AnalystEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeveloperEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsIncludedInBonus = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    JiraProjectNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JiraTaskNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProjectManagerEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TotalEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Projects_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Projects_DzdStatuses_DzdStatusId",
                        column: x => x.DzdStatusId,
                        principalTable: "DzdStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Projects_JiraStatuses_JiraStatusId",
                        column: x => x.JiraStatusId,
                        principalTable: "JiraStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Projects_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Projects_Salesforces_IntertechTeamId",
                        column: x => x.IntertechTeamId,
                        principalTable: "Salesforces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Projects_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Projects_Users_AnalystId",
                        column: x => x.AnalystId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Users_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Users_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTrainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    TrainingId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Evaluation = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTrainings_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTrainings_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTrainings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentCategoryId = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    StoredFileId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReviewStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDocuments_StoredFiles_StoredFileId",
                        column: x => x.StoredFileId,
                        principalTable: "StoredFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDocuments_UserDocumentCategories_DocumentCategoryId",
                        column: x => x.DocumentCategoryId,
                        principalTable: "UserDocumentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserDocuments_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDocuments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Itsms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssigneeId = table.Column<int>(type: "int", nullable: true),
                    BankId = table.Column<int>(type: "int", nullable: true),
                    IssuePriorityId = table.Column<int>(type: "int", nullable: true),
                    IssueStatusId = table.Column<int>(type: "int", nullable: true),
                    IssueTypeId = table.Column<int>(type: "int", nullable: true),
                    ItsmPaymentTypeId = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    ResolutionId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssueKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itsms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itsms_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Itsms_IssuePaymentTypes_ItsmPaymentTypeId",
                        column: x => x.ItsmPaymentTypeId,
                        principalTable: "IssuePaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Itsms_IssuePriorities_IssuePriorityId",
                        column: x => x.IssuePriorityId,
                        principalTable: "IssuePriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Itsms_IssueStatuses_IssueStatusId",
                        column: x => x.IssueStatusId,
                        principalTable: "IssueStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Itsms_IssueTypes_IssueTypeId",
                        column: x => x.IssueTypeId,
                        principalTable: "IssueTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Itsms_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Itsms_Resolutions_ResolutionId",
                        column: x => x.ResolutionId,
                        principalTable: "Resolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Itsms_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Itsms_Users_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Itsms_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDocuments_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectDocuments_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_DocumentStoredFileId",
                table: "ChecklistItems",
                column: "DocumentStoredFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_BankName",
                table: "Banks",
                column: "BankName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_ModifiedById",
                table: "Banks",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_AnalystId",
                table: "Bids",
                column: "AnalystId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_BankId",
                table: "Bids",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_DepartmentId",
                table: "Bids",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_DeveloperId",
                table: "Bids",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_DzdStatusId",
                table: "Bids",
                column: "DzdStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_IntertechTeamId",
                table: "Bids",
                column: "IntertechTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_JiraProjectNo",
                table: "Bids",
                column: "JiraProjectNo",
                unique: true,
                filter: "[JiraProjectNo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_ModifiedById",
                table: "Bids",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_PeriodId",
                table: "Bids",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_ProjectManagerId",
                table: "Bids",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_TeamId",
                table: "Bids",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultDocuments_DocumentName",
                table: "DefaultDocuments",
                column: "DocumentName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DefaultDocuments_ModifiedById",
                table: "DefaultDocuments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DzdStatuses_DzdStatusName",
                table: "DzdStatuses",
                column: "DzdStatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DzdStatuses_ModifiedById",
                table: "DzdStatuses",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExCompanyHistories_ModifiedById",
                table: "ExCompanyHistories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExCompanyHistories_UserId",
                table: "ExCompanyHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_NextStepId",
                table: "Grades",
                column: "NextStepId");

            migrationBuilder.CreateIndex(
                name: "IX_HeadLeadCoefficients_JobId",
                table: "HeadLeadCoefficients",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_HeadLeadCoefficients_ModifiedById",
                table: "HeadLeadCoefficients",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_HeadLeadCoefficients_PeriodId_JobId",
                table: "HeadLeadCoefficients",
                columns: new[] { "PeriodId", "JobId" },
                unique: true,
                filter: "[PeriodId] IS NOT NULL AND [JobId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IssuePaymentTypes_ModifiedById",
                table: "IssuePaymentTypes",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_IssuePaymentTypes_PaymentTypeName_PeriodId",
                table: "IssuePaymentTypes",
                columns: new[] { "PaymentTypeName", "PeriodId" },
                unique: true,
                filter: "[PeriodId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IssuePaymentTypes_PeriodId",
                table: "IssuePaymentTypes",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuePriorities_ModifiedById",
                table: "IssuePriorities",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_IssuePriorities_PriorityName",
                table: "IssuePriorities",
                column: "PriorityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IssueStatuses_ModifiedById",
                table: "IssueStatuses",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_IssueStatuses_StatusName",
                table: "IssueStatuses",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IssueTypes_ModifiedById",
                table: "IssueTypes",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_IssueTypes_TypeName",
                table: "IssueTypes",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Itsms_AssigneeId",
                table: "Itsms",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Itsms_BankId",
                table: "Itsms",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Itsms_IssueKey",
                table: "Itsms",
                column: "IssueKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Itsms_IssuePriorityId",
                table: "Itsms",
                column: "IssuePriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Itsms_IssueStatusId",
                table: "Itsms",
                column: "IssueStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Itsms_IssueTypeId",
                table: "Itsms",
                column: "IssueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Itsms_ItsmPaymentTypeId",
                table: "Itsms",
                column: "ItsmPaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Itsms_ModifiedById",
                table: "Itsms",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Itsms_PeriodId",
                table: "Itsms",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Itsms_ResolutionId",
                table: "Itsms",
                column: "ResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Itsms_TeamId",
                table: "Itsms",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_JiraStatuses_JiraStatusName",
                table: "JiraStatuses",
                column: "JiraStatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JiraStatuses_ModifiedById",
                table: "JiraStatuses",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_ModifiedById",
                table: "Periods",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_PeriodName",
                table: "Periods",
                column: "PeriodName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBonusCoefficients_ModifiedById",
                table: "ProjectBonusCoefficients",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBonusCoefficients_PeriodId",
                table: "ProjectBonusCoefficients",
                column: "PeriodId",
                unique: true,
                filter: "[PeriodId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocuments_ModifiedById",
                table: "ProjectDocuments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocuments_ProjectId",
                table: "ProjectDocuments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvoices_BankId",
                table: "ProjectInvoices",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvoices_DepartmentId",
                table: "ProjectInvoices",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvoices_DzdStatusId",
                table: "ProjectInvoices",
                column: "DzdStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvoices_EFaturaNumber",
                table: "ProjectInvoices",
                column: "EFaturaNumber",
                unique: true,
                filter: "[EFaturaNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvoices_ModifiedById",
                table: "ProjectInvoices",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvoices_PayrollLocationId",
                table: "ProjectInvoices",
                column: "PayrollLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvoices_PeriodId",
                table: "ProjectInvoices",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvoices_PurchaseInvoiceNumber",
                table: "ProjectInvoices",
                column: "PurchaseInvoiceNumber",
                unique: true,
                filter: "[PurchaseInvoiceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AnalystId",
                table: "Projects",
                column: "AnalystId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_BankId",
                table: "Projects",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DepartmentId",
                table: "Projects",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DeveloperId",
                table: "Projects",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DzdStatusId",
                table: "Projects",
                column: "DzdStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IntertechTeamId",
                table: "Projects",
                column: "IntertechTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_JiraProjectNo_JiraTaskNo",
                table: "Projects",
                columns: new[] { "JiraProjectNo", "JiraTaskNo" },
                unique: true,
                filter: "[JiraProjectNo] IS NOT NULL AND [JiraTaskNo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_JiraStatusId",
                table: "Projects",
                column: "JiraStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ModifiedById",
                table: "Projects",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PeriodId",
                table: "Projects",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TeamId",
                table: "Projects",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ModifiedById",
                table: "Resolutions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ResolutionName",
                table: "Resolutions",
                column: "ResolutionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salesforces_ModifiedById",
                table: "Salesforces",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Salesforces_PayrollLocationId",
                table: "Salesforces",
                column: "PayrollLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Salesforces_TaskTeam_TaskPo",
                table: "Salesforces",
                columns: new[] { "TaskTeam", "TaskPo" },
                unique: true,
                filter: "[TaskPo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TargetEfforts_ModifiedById",
                table: "TargetEfforts",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_TargetEfforts_PeriodId_UserId",
                table: "TargetEfforts",
                columns: new[] { "PeriodId", "UserId" },
                unique: true,
                filter: "[PeriodId] IS NOT NULL AND [UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TargetEfforts_UserId",
                table: "TargetEfforts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_ModifiedById",
                table: "Trainings",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_Name",
                table: "Trainings",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_ModifiedById",
                table: "UserAvatars",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_StorageId",
                table: "UserAvatars",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_UserId",
                table: "UserAvatars",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDocumentCategories_ModifiedById",
                table: "UserDocumentCategories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocumentCategories_Name",
                table: "UserDocumentCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_DocumentCategoryId",
                table: "UserDocuments",
                column: "DocumentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_ModifiedById",
                table: "UserDocuments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_StoredFileId",
                table: "UserDocuments",
                column: "StoredFileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_UserId_FileName",
                table: "UserDocuments",
                columns: new[] { "UserId", "FileName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGradeHistories_UserId",
                table: "UserGradeHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainings_ModifiedById",
                table: "UserTrainings",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainings_TrainingId",
                table: "UserTrainings",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainings_UserId_TrainingId",
                table: "UserTrainings",
                columns: new[] { "UserId", "TrainingId" },
                unique: true,
                filter: "[UserId] IS NOT NULL AND [TrainingId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_StoredFiles_DocumentStoredFileId",
                table: "ChecklistItems",
                column: "DocumentStoredFileId",
                principalTable: "StoredFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StoredFiles_Users_ModifiedById",
                table: "StoredFiles",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
