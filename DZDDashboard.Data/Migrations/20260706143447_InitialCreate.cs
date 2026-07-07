using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CareerPathRuleJobs",
                columns: table => new
                {
                    CareerPathRuleId = table.Column<int>(type: "integer", nullable: false),
                    JobId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerPathRuleJobs", x => new { x.CareerPathRuleId, x.JobId });
                });

            migrationBuilder.CreateTable(
                name: "CareerPathRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CareerPathId = table.Column<int>(type: "integer", nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    MinRoleTimeMonth = table.Column<int>(type: "integer", nullable: true),
                    MinRoleTimeYear = table.Column<int>(type: "integer", nullable: true),
                    MinExperienceMonth = table.Column<int>(type: "integer", nullable: true),
                    MinExperienceYear = table.Column<int>(type: "integer", nullable: true),
                    ManagerPerformanceEvaluation = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AssessmentCenterApplication = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TechnicalInterview = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CaseStudy = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    EnglishProficiency = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ProjectObjective = table.Column<int>(type: "integer", nullable: true),
                    CommitteeApproval = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SalaryIncreasePercent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    PrivatePensionInsuranceAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    PrivatePensionInsuranceCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    EmployerContributionUpperLimitAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    EmployerContributionUpperLimitCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    MealAllowanceAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MealAllowanceCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerPathRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CareerPaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerPaths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OnboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    OffboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedById = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistStepTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProcessTemplateId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistStepTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
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
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    DeadlineDays = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Relationship = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<byte[]>(type: "bytea", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LifecycleAuditLogEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OnboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    OffboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Detail = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    PerformedById = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifecycleAuditLogEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OffboardingProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TemplateId = table.Column<int>(type: "integer", nullable: false),
                    TemplateName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ExitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffboardingProcesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ManagerId = table.Column<int>(type: "integer", nullable: true),
                    TemplateId = table.Column<int>(type: "integer", nullable: false),
                    TemplateName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingProcesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationPositions_OrganizationPositions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "OrganizationPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Location = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OnboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    OffboardingProcessId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FileId = table.Column<int>(type: "integer", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UploadedById = table.Column<int>(type: "integer", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReviewedById = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "ProcessTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Kind = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: true),
                    DepartmentId = table.Column<int>(type: "integer", nullable: true),
                    TeamId = table.Column<int>(type: "integer", nullable: true),
                    PayrollLocationId = table.Column<int>(type: "integer", nullable: true),
                    OrganizationPositionId = table.Column<int>(type: "integer", nullable: true),
                    ReportsToId = table.Column<int>(type: "integer", nullable: true),
                    JobId = table.Column<int>(type: "integer", nullable: true),
                    Grade = table.Column<int>(type: "integer", nullable: true),
                    CareerPathId = table.Column<int>(type: "integer", nullable: true),
                    AvatarId = table.Column<int>(type: "integer", nullable: true),
                    EntraObjectId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AvatarColorIndex = table.Column<int>(type: "integer", nullable: true),
                    RegistrationNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PositionStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PositionUpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ContractType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ContractEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WorkModel = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LifecycleStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PersonalEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    PersonalPhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Nationality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CitizenshipNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DisabilityStatus = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DisabilityDegree = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MaritalStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SpouseFullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LegalAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LegalAddressCity = table.Column<string>(type: "text", nullable: true),
                    LegalAddressCountry = table.Column<string>(type: "text", nullable: true),
                    CurrentAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CurrentAddressChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_CareerPaths_CareerPathId",
                        column: x => x.CareerPathId,
                        principalTable: "CareerPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Files_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_OrganizationPositions_OrganizationPositionId",
                        column: x => x.OrganizationPositionId,
                        principalTable: "OrganizationPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_PayrollLocations_PayrollLocationId",
                        column: x => x.PayrollLocationId,
                        principalTable: "PayrollLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_ReportsToId",
                        column: x => x.ReportsToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UdemyCourseActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    UdemyUserId = table.Column<long>(type: "bigint", nullable: false),
                    UserEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserExternalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    CourseTitle = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    CourseCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CourseDurationMinutes = table.Column<double>(type: "double precision", nullable: true),
                    CompletionRatio = table.Column<double>(type: "double precision", nullable: false),
                    EnrollDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastAccessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsAssigned = table.Column<bool>(type: "boolean", nullable: false),
                    AssignedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LastSyncedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UdemyCourseActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UdemyCourseActivities_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UdemyCourseActivities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserAdditionalPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaymentType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Period = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdditionalPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAdditionalPayments_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAdditionalPayments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBenefitRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BenefitType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BenefitName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Period = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProviderName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    EmployeeContributionAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    EmployerContributionAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    PolicyNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBenefitRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBenefitRecords_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserBenefitRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserChildren",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChildren", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChildren_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserChildren_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCvDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    FileId = table.Column<int>(type: "integer", nullable: true),
                    ReviewStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    ReviewNote = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "UserDeductions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeductionType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Period = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDeductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDeductions_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDeductions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserEducationHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EducationLevel = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Institution = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Program = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    GraduationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEducationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEducationHistories_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserEducationHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPositionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    JobTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DepartmentName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    TeamName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Grade = table.Column<int>(type: "integer", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPositionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPositionHistories_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPositionHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSalaryHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PayType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Period = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PayrollCycle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    NotesModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSalaryHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSalaryHistories_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSalaryHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBenefitDependents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    DependentName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RelationType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BenefitRecordId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBenefitDependents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBenefitDependents_UserBenefitRecords_BenefitRecordId",
                        column: x => x.BenefitRecordId,
                        principalTable: "UserBenefitRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBenefitDependents_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ProcessTemplates",
                columns: new[] { "Id", "CreatedAt", "Kind", "ModifiedAt", "ModifiedById", "Name", "Sequence" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Onboarding", null, null, "General Onboarding", 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Offboarding", null, null, "Resignation", 1 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Offboarding", null, null, "Termination", 2 }
                });

            migrationBuilder.InsertData(
                table: "ChecklistStepTemplates",
                columns: new[] { "Id", "CreatedAt", "IsRequired", "ModifiedAt", "ModifiedById", "ProcessTemplateId", "Sequence", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, 1, "Contract prepared and signed" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, 2, "Social security registration completed" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, 3, "Accountant notified" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, 4, "Private Pension System (BES) account opened" },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, 5, "Private Health Insurance (ÖSS) opened" },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, 6, "Computer delivered" },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 2, 1, "Resignation letter received" },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 2, 2, "Social security exit processed" },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 2, 3, "Access revoked" },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 2, 4, "Asset return confirmed" },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 2, 5, "Final settlement calculated" },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 3, 1, "Justification documented" },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 3, 2, "Settlement/severance calculated" },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 3, 3, "Social security exit processed" },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 3, 4, "Access revoked" },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 3, 5, "Asset return confirmed" }
                });

            migrationBuilder.InsertData(
                table: "DocumentTemplates",
                columns: new[] { "Id", "CreatedAt", "DeadlineDays", "IsRequired", "ModifiedAt", "ModifiedById", "Name", "ProcessTemplateId", "Sequence" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, true, null, null, "İkametgâh", 1, 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, true, null, null, "Diploma", 1, 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, true, null, null, "Nüfus Kayıt Örneği", 1, 3 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, true, null, null, "TC Kimlik Kartı Fotokopisi", 1, 4 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, false, null, null, "Adli Sicil Kaydı", 1, 5 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, false, null, null, "Akciğer grafisi, hemogram ve göz raporu", 1, 6 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, true, null, null, "Akbank Maaş Hesabı Bilgisi", 1, 7 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, null, null, "İstifa Dilekçesi", 2, 1 },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, true, null, null, "Zimmet İade Tutanağı", 2, 2 },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, null, null, "Fesih Bildirimi", 3, 1 },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, true, null, null, "Zimmet İade Tutanağı", 3, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareerPathRuleJobs_JobId",
                table: "CareerPathRuleJobs",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_CareerPathRules_CareerPathId_Grade",
                table: "CareerPathRules",
                columns: new[] { "CareerPathId", "Grade" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CareerPathRules_ModifiedById",
                table: "CareerPathRules",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CareerPaths_ModifiedById",
                table: "CareerPaths",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_CompletedById",
                table: "ChecklistItems",
                column: "CompletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_ModifiedById",
                table: "ChecklistItems",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_OffboardingProcessId",
                table: "ChecklistItems",
                column: "OffboardingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_OnboardingProcessId",
                table: "ChecklistItems",
                column: "OnboardingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistStepTemplates_ModifiedById",
                table: "ChecklistStepTemplates",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistStepTemplates_ProcessTemplateId",
                table: "ChecklistStepTemplates",
                column: "ProcessTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ModifiedById",
                table: "Companies",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name",
                table: "Companies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId",
                table: "Departments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ModifiedById",
                table: "Departments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_ModifiedById",
                table: "DocumentTemplates",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_ProcessTemplateId",
                table: "DocumentTemplates",
                column: "ProcessTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_ModifiedById",
                table: "EmergencyContacts",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_UserId",
                table: "EmergencyContacts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ModifiedById",
                table: "Files",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ModifiedById",
                table: "Jobs",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Title",
                table: "Jobs",
                column: "Title",
                unique: true);

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
                name: "IX_OffboardingProcesses_ModifiedById",
                table: "OffboardingProcesses",
                column: "ModifiedById");

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
                name: "IX_OnboardingProcesses_ManagerId",
                table: "OnboardingProcesses",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingProcesses_ModifiedById",
                table: "OnboardingProcesses",
                column: "ModifiedById");

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
                name: "IX_OrganizationPositions_ModifiedById",
                table: "OrganizationPositions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPositions_ParentId",
                table: "OrganizationPositions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLocations_Location",
                table: "PayrollLocations",
                column: "Location",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLocations_ModifiedById",
                table: "PayrollLocations",
                column: "ModifiedById");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplates_Kind",
                table: "ProcessTemplates",
                column: "Kind");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplates_ModifiedById",
                table: "ProcessTemplates",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_DepartmentId",
                table: "Teams",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ModifiedById",
                table: "Teams",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Name",
                table: "Teams",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UdemyCourseActivities_ModifiedById",
                table: "UdemyCourseActivities",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UdemyCourseActivities_UdemyUserId_CourseId",
                table: "UdemyCourseActivities",
                columns: new[] { "UdemyUserId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UdemyCourseActivities_UserId",
                table: "UdemyCourseActivities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalPayments_ModifiedById",
                table: "UserAdditionalPayments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalPayments_UserId_Period",
                table: "UserAdditionalPayments",
                columns: new[] { "UserId", "Period" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefitDependents_BenefitRecordId",
                table: "UserBenefitDependents",
                column: "BenefitRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefitDependents_ModifiedById",
                table: "UserBenefitDependents",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefitRecords_ModifiedById",
                table: "UserBenefitRecords",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefitRecords_UserId_BenefitType_StartDate",
                table: "UserBenefitRecords",
                columns: new[] { "UserId", "BenefitType", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_UserChildren_ModifiedById",
                table: "UserChildren",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserChildren_UserId",
                table: "UserChildren",
                column: "UserId");

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
                name: "IX_UserDeductions_ModifiedById",
                table: "UserDeductions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeductions_UserId_StartDate",
                table: "UserDeductions",
                columns: new[] { "UserId", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_UserEducationHistories_ModifiedById",
                table: "UserEducationHistories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserEducationHistories_UserId",
                table: "UserEducationHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPositionHistories_ModifiedById",
                table: "UserPositionHistories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPositionHistories_UserId",
                table: "UserPositionHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarId",
                table: "Users",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CareerPathId",
                table: "Users",
                column: "CareerPathId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EntraObjectId",
                table: "Users",
                column: "EntraObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_JobId",
                table: "Users",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModifiedById",
                table: "Users",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedEmail",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationPositionId",
                table: "Users",
                column: "OrganizationPositionId",
                unique: true,
                filter: "\"OrganizationPositionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PayrollLocationId",
                table: "Users",
                column: "PayrollLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReportsToId",
                table: "Users",
                column: "ReportsToId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Slug",
                table: "Users",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamId",
                table: "Users",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSalaryHistories_ModifiedById",
                table: "UserSalaryHistories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserSalaryHistories_UserId_StartDate",
                table: "UserSalaryHistories",
                columns: new[] { "UserId", "StartDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_CareerPathRuleJobs_CareerPathRules_CareerPathRuleId",
                table: "CareerPathRuleJobs",
                column: "CareerPathRuleId",
                principalTable: "CareerPathRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CareerPathRuleJobs_Jobs_JobId",
                table: "CareerPathRuleJobs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CareerPathRules_CareerPaths_CareerPathId",
                table: "CareerPathRules",
                column: "CareerPathId",
                principalTable: "CareerPaths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_ChecklistItems_OffboardingProcesses_OffboardingProcessId",
                table: "ChecklistItems",
                column: "OffboardingProcessId",
                principalTable: "OffboardingProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_OnboardingProcesses_OnboardingProcessId",
                table: "ChecklistItems",
                column: "OnboardingProcessId",
                principalTable: "OnboardingProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_Users_CompletedById",
                table: "ChecklistItems",
                column: "CompletedById",
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
                name: "FK_ChecklistStepTemplates_ProcessTemplates_ProcessTemplateId",
                table: "ChecklistStepTemplates",
                column: "ProcessTemplateId",
                principalTable: "ProcessTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_DocumentTemplates_ProcessTemplates_ProcessTemplateId",
                table: "DocumentTemplates",
                column: "ProcessTemplateId",
                principalTable: "ProcessTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_EmergencyContacts_Users_UserId",
                table: "EmergencyContacts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_LifecycleAuditLogEntries_OffboardingProcesses_OffboardingPr~",
                table: "LifecycleAuditLogEntries",
                column: "OffboardingProcessId",
                principalTable: "OffboardingProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LifecycleAuditLogEntries_OnboardingProcesses_OnboardingProc~",
                table: "LifecycleAuditLogEntries",
                column: "OnboardingProcessId",
                principalTable: "OnboardingProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LifecycleAuditLogEntries_Users_PerformedById",
                table: "LifecycleAuditLogEntries",
                column: "PerformedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OffboardingProcesses_ProcessTemplates_TemplateId",
                table: "OffboardingProcesses",
                column: "TemplateId",
                principalTable: "ProcessTemplates",
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
                name: "FK_OffboardingProcesses_Users_UserId",
                table: "OffboardingProcesses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OnboardingProcesses_ProcessTemplates_TemplateId",
                table: "OnboardingProcesses",
                column: "TemplateId",
                principalTable: "ProcessTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OnboardingProcesses_Users_ManagerId",
                table: "OnboardingProcesses",
                column: "ManagerId",
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
                name: "FK_OnboardingProcesses_Users_UserId",
                table: "OnboardingProcesses",
                column: "UserId",
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
                name: "FK_ProcessDocuments_Users_ReviewedById",
                table: "ProcessDocuments",
                column: "ReviewedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDocuments_Users_UploadedById",
                table: "ProcessDocuments",
                column: "UploadedById",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Jobs_JobId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CareerPaths_CareerPathId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_ModifiedById",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_ModifiedById",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_ModifiedById",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationPositions_Users_ModifiedById",
                table: "OrganizationPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollLocations_Users_ModifiedById",
                table: "PayrollLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Users_ModifiedById",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "CareerPathRuleJobs");

            migrationBuilder.DropTable(
                name: "ChecklistItems");

            migrationBuilder.DropTable(
                name: "ChecklistStepTemplates");

            migrationBuilder.DropTable(
                name: "DocumentTemplates");

            migrationBuilder.DropTable(
                name: "EmergencyContacts");

            migrationBuilder.DropTable(
                name: "LifecycleAuditLogEntries");

            migrationBuilder.DropTable(
                name: "ProcessDocuments");

            migrationBuilder.DropTable(
                name: "UdemyCourseActivities");

            migrationBuilder.DropTable(
                name: "UserAdditionalPayments");

            migrationBuilder.DropTable(
                name: "UserBenefitDependents");

            migrationBuilder.DropTable(
                name: "UserChildren");

            migrationBuilder.DropTable(
                name: "UserCvDocuments");

            migrationBuilder.DropTable(
                name: "UserDeductions");

            migrationBuilder.DropTable(
                name: "UserEducationHistories");

            migrationBuilder.DropTable(
                name: "UserPositionHistories");

            migrationBuilder.DropTable(
                name: "UserSalaryHistories");

            migrationBuilder.DropTable(
                name: "CareerPathRules");

            migrationBuilder.DropTable(
                name: "OffboardingProcesses");

            migrationBuilder.DropTable(
                name: "OnboardingProcesses");

            migrationBuilder.DropTable(
                name: "UserBenefitRecords");

            migrationBuilder.DropTable(
                name: "ProcessTemplates");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "CareerPaths");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "OrganizationPositions");

            migrationBuilder.DropTable(
                name: "PayrollLocations");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
