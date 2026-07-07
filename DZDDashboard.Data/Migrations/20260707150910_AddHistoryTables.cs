using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalPaymentHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    PaymentType = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalPaymentHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "BenefitDependentHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    DependentName = table.Column<string>(type: "text", nullable: true),
                    RelationType = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BenefitRecordId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitDependentHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "BenefitRecordHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    BenefitType = table.Column<string>(type: "text", nullable: false),
                    BenefitName = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProviderName = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    EmployeeContributionAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    EmployerContributionAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    PolicyNumber = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitRecordHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "CareerPathHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerPathHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "CareerPathRuleHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CareerPathId = table.Column<int>(type: "integer", nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    MinRoleTimeMonths = table.Column<int>(type: "integer", nullable: true),
                    MinRoleTimeYears = table.Column<int>(type: "integer", nullable: true),
                    MinExperienceMonths = table.Column<int>(type: "integer", nullable: true),
                    MinExperienceYears = table.Column<int>(type: "integer", nullable: true),
                    ManagerPerformanceEvaluation = table.Column<bool>(type: "boolean", nullable: false),
                    AssessmentCenterApplication = table.Column<bool>(type: "boolean", nullable: false),
                    TechnicalInterview = table.Column<bool>(type: "boolean", nullable: false),
                    CaseStudy = table.Column<bool>(type: "boolean", nullable: false),
                    EnglishProficiency = table.Column<bool>(type: "boolean", nullable: false),
                    ProjectObjective = table.Column<int>(type: "integer", nullable: true),
                    CommitteeApproval = table.Column<bool>(type: "boolean", nullable: false),
                    SalaryIncreasePercent = table.Column<decimal>(type: "numeric", nullable: true),
                    PrivatePensionInsuranceAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    PrivatePensionInsuranceCurrency = table.Column<string>(type: "text", nullable: true),
                    EmployerContributionUpperLimitAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    EmployerContributionUpperLimitCurrency = table.Column<string>(type: "text", nullable: true),
                    MealAllowanceAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    MealAllowanceCurrency = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerPathRuleHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistItemHistory",
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
                    Title = table.Column<string>(type: "text", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedById = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
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
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ProcessTemplateId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistStepTemplateHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "ChildInfoHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildInfoHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "DeductionHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    DeductionType = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeductionHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplateHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ProcessTemplateId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    DeadlineDays = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplateHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "EducationHistoryHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EducationLevel = table.Column<string>(type: "text", nullable: true),
                    Institution = table.Column<string>(type: "text", nullable: true),
                    Program = table.Column<string>(type: "text", nullable: true),
                    GraduationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationHistoryHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyContactHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Relationship = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContactHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "JobHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "OffboardingProcessHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TemplateId = table.Column<int>(type: "integer", nullable: false),
                    TemplateName = table.Column<string>(type: "text", nullable: false),
                    ExitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
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
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ManagerId = table.Column<int>(type: "integer", nullable: true),
                    TemplateId = table.Column<int>(type: "integer", nullable: false),
                    TemplateName = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingProcessHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationPositionHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationPositionHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "PayrollLocationHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollLocationHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "PositionHistoryHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    JobTitle = table.Column<string>(type: "text", nullable: true),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    DepartmentName = table.Column<string>(type: "text", nullable: true),
                    TeamName = table.Column<string>(type: "text", nullable: true),
                    Grade = table.Column<int>(type: "integer", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangeType = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionHistoryHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessDocumentHistory",
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
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    ContentType = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_ProcessDocumentHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessTemplateHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Kind = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTemplateHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "SalaryHistoryHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    PayType = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    PayrollCycle = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    NotesModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryHistoryHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "StoredFileHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredFileHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "TeamHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    DepartmentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "UdemyCourseActivityHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    UdemyUserId = table.Column<long>(type: "bigint", nullable: false),
                    UserEmail = table.Column<string>(type: "text", nullable: false),
                    UserExternalId = table.Column<string>(type: "text", nullable: true),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    CourseTitle = table.Column<string>(type: "text", nullable: false),
                    CourseCategory = table.Column<string>(type: "text", nullable: true),
                    CourseDurationMinutes = table.Column<double>(type: "double precision", nullable: true),
                    CompletionRatio = table.Column<double>(type: "double precision", nullable: false),
                    EnrollDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastAccessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsAssigned = table.Column<bool>(type: "boolean", nullable: false),
                    AssignedBy = table.Column<string>(type: "text", nullable: true),
                    LastSyncedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UdemyCourseActivityHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "UserCvDocumentHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    FileId = table.Column<int>(type: "integer", nullable: true),
                    ReviewStatus = table.Column<string>(type: "text", nullable: false),
                    ReviewNote = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCvDocumentHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "UserHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HistoryRecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HistoryRecordedById = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EntraObjectId = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    AvatarColorIndex = table.Column<int>(type: "integer", nullable: true),
                    RegistrationNumber = table.Column<string>(type: "text", nullable: true),
                    UserStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PositionStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PositionUpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ContractType = table.Column<string>(type: "text", nullable: true),
                    ContractEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WorkModel = table.Column<string>(type: "text", nullable: true),
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
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PersonalEmail = table.Column<string>(type: "text", nullable: true),
                    PersonalPhoneNumber = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    Nationality = table.Column<string>(type: "text", nullable: true),
                    CitizenshipNumber = table.Column<string>(type: "text", nullable: true),
                    DisabilityStatus = table.Column<bool>(type: "boolean", nullable: false),
                    DisabilityDegree = table.Column<string>(type: "text", nullable: true),
                    MaritalStatus = table.Column<string>(type: "text", nullable: true),
                    SpouseFullName = table.Column<string>(type: "text", nullable: true),
                    LegalAddress = table.Column<string>(type: "text", nullable: true),
                    LegalAddressCity = table.Column<string>(type: "text", nullable: true),
                    LegalAddressCountry = table.Column<string>(type: "text", nullable: true),
                    CurrentAddress = table.Column<string>(type: "text", nullable: true),
                    CurrentAddressChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    LifecycleStatus = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistory", x => x.HistoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalPaymentHistory_Id",
                table: "AdditionalPaymentHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitDependentHistory_Id",
                table: "BenefitDependentHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitRecordHistory_Id",
                table: "BenefitRecordHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CareerPathHistory_Id",
                table: "CareerPathHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CareerPathRuleHistory_Id",
                table: "CareerPathRuleHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItemHistory_Id",
                table: "ChecklistItemHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistStepTemplateHistory_Id",
                table: "ChecklistStepTemplateHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChildInfoHistory_Id",
                table: "ChildInfoHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyHistory_Id",
                table: "CompanyHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DeductionHistory_Id",
                table: "DeductionHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentHistory_Id",
                table: "DepartmentHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplateHistory_Id",
                table: "DocumentTemplateHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EducationHistoryHistory_Id",
                table: "EducationHistoryHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContactHistory_Id",
                table: "EmergencyContactHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_JobHistory_Id",
                table: "JobHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OffboardingProcessHistory_Id",
                table: "OffboardingProcessHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingProcessHistory_Id",
                table: "OnboardingProcessHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPositionHistory_Id",
                table: "OrganizationPositionHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLocationHistory_Id",
                table: "PayrollLocationHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PositionHistoryHistory_Id",
                table: "PositionHistoryHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDocumentHistory_Id",
                table: "ProcessDocumentHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplateHistory_Id",
                table: "ProcessTemplateHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryHistoryHistory_Id",
                table: "SalaryHistoryHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StoredFileHistory_Id",
                table: "StoredFileHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TeamHistory_Id",
                table: "TeamHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UdemyCourseActivityHistory_Id",
                table: "UdemyCourseActivityHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserCvDocumentHistory_Id",
                table: "UserCvDocumentHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistory_Id",
                table: "UserHistory",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalPaymentHistory");

            migrationBuilder.DropTable(
                name: "BenefitDependentHistory");

            migrationBuilder.DropTable(
                name: "BenefitRecordHistory");

            migrationBuilder.DropTable(
                name: "CareerPathHistory");

            migrationBuilder.DropTable(
                name: "CareerPathRuleHistory");

            migrationBuilder.DropTable(
                name: "ChecklistItemHistory");

            migrationBuilder.DropTable(
                name: "ChecklistStepTemplateHistory");

            migrationBuilder.DropTable(
                name: "ChildInfoHistory");

            migrationBuilder.DropTable(
                name: "CompanyHistory");

            migrationBuilder.DropTable(
                name: "DeductionHistory");

            migrationBuilder.DropTable(
                name: "DepartmentHistory");

            migrationBuilder.DropTable(
                name: "DocumentTemplateHistory");

            migrationBuilder.DropTable(
                name: "EducationHistoryHistory");

            migrationBuilder.DropTable(
                name: "EmergencyContactHistory");

            migrationBuilder.DropTable(
                name: "JobHistory");

            migrationBuilder.DropTable(
                name: "OffboardingProcessHistory");

            migrationBuilder.DropTable(
                name: "OnboardingProcessHistory");

            migrationBuilder.DropTable(
                name: "OrganizationPositionHistory");

            migrationBuilder.DropTable(
                name: "PayrollLocationHistory");

            migrationBuilder.DropTable(
                name: "PositionHistoryHistory");

            migrationBuilder.DropTable(
                name: "ProcessDocumentHistory");

            migrationBuilder.DropTable(
                name: "ProcessTemplateHistory");

            migrationBuilder.DropTable(
                name: "SalaryHistoryHistory");

            migrationBuilder.DropTable(
                name: "StoredFileHistory");

            migrationBuilder.DropTable(
                name: "TeamHistory");

            migrationBuilder.DropTable(
                name: "UdemyCourseActivityHistory");

            migrationBuilder.DropTable(
                name: "UserCvDocumentHistory");

            migrationBuilder.DropTable(
                name: "UserHistory");
        }
    }
}
