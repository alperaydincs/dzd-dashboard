using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankId = table.Column<int>(type: "int", nullable: true),
                    JiraProjectNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DzdStatusId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    ProjectManagerId = table.Column<int>(type: "int", nullable: true),
                    AnalystId = table.Column<int>(type: "int", nullable: true),
                    DeveloperId = table.Column<int>(type: "int", nullable: true),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AnalysisDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UatDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TshirtSize = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IntertechTeamId = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "CareerMapRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    MinExperienceMonth = table.Column<int>(type: "int", nullable: true),
                    MinExperienceYear = table.Column<int>(type: "int", nullable: true),
                    MinRoleTimeMonth = table.Column<int>(type: "int", nullable: true),
                    MinRoleTimeyear = table.Column<int>(type: "int", nullable: true),
                    ManagerPerformanceEvaluation = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AssesmentCenterApplication = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TechnicalInterview = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CaseStudy = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EnglishProficiency = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ProjectObjective = table.Column<int>(type: "int", nullable: true),
                    CommitteeApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerMapRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DefaultDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DzdStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DzdStatusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DzdStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExCompanyHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExCompanyHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeadLeadCoefficients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    JobId = table.Column<int>(type: "int", nullable: true),
                    Coefficient = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadLeadCoefficients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IssuePaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentTypeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Coefficient = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuePaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IssuePriorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriorityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuePriorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IssueStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IssueTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Itsms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueTypeId = table.Column<int>(type: "int", nullable: true),
                    BankId = table.Column<int>(type: "int", nullable: true),
                    IssueKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AsigneeId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    ResolutionId = table.Column<int>(type: "int", nullable: true),
                    IssuePriorityId = table.Column<int>(type: "int", nullable: true),
                    IssueStatusId = table.Column<int>(type: "int", nullable: true),
                    ItsmPaymentTypeId = table.Column<int>(type: "int", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "JiraStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JiraStatusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JiraStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayrollLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBonusCoefficients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Coefficient = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "ProjectDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    DocumentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankId = table.Column<int>(type: "int", nullable: true),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DzdStatusId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    TotalEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    PurchaseInvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EFaturaNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    JiraProjectNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JiraTaskNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PartialInvoice = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatIncludedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayrollLocationId = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankId = table.Column<int>(type: "int", nullable: true),
                    JiraProjectNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JiraTaskNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DzdStatusId = table.Column<int>(type: "int", nullable: true),
                    JiraStatusId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    TotalEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeveloperId = table.Column<int>(type: "int", nullable: true),
                    DeveloperEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AnalystId = table.Column<int>(type: "int", nullable: true),
                    AnalystEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectManagerId = table.Column<int>(type: "int", nullable: true),
                    ProjectManagerEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IntertechTeamId = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsIncludedInBonus = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Resolutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResolutionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resolutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Salesforces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTeam = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TaskPo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsSuitable = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Info = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    PayrollLocationId = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "TargetEfforts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Target = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompletedTarget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingTarget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectBonusAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItsmBonusAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ManagerBonusEffort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ManagerBonusAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBonusAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    InstructorName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InstructorCompanyDetails = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RepeatFrequency = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDocumentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocumentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DocumentCategoryId = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDocuments_UserDocumentCategories_DocumentCategoryId",
                        column: x => x.DocumentCategoryId,
                        principalTable: "UserDocumentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedUsername = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    UserGroupId = table.Column<int>(type: "int", nullable: true),
                    JobId = table.Column<int>(type: "int", nullable: true),
                    ReportsToId = table.Column<int>(type: "int", nullable: true),
                    Grade = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UserStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnitName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PayrollLocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BachelorsProgramName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BachelorsGraduatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MastersCollageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MastersProgramName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MastersGraduatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DoctoraPhdStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AvatarDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalProcessUnit = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    CitizenshipNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    EmployeeGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
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
                        name: "FK_Users_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
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
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTrainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Evaluation = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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
                name: "IX_CareerMapRules_JobId_Grade",
                table: "CareerMapRules",
                columns: new[] { "JobId", "Grade" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CareerMapRules_ModifiedById",
                table: "CareerMapRules",
                column: "ModifiedById");

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
                name: "IX_Departments_DepartmentName",
                table: "Departments",
                column: "DepartmentName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ModifiedById",
                table: "Departments",
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
                name: "IX_Itsms_AsigneeId",
                table: "Itsms",
                column: "AsigneeId");

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
                name: "IX_Jobs_ModifiedById",
                table: "Jobs",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Title",
                table: "Jobs",
                column: "Title",
                unique: true);

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
                name: "IX_Roles_ModifiedById",
                table: "Roles",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
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
                name: "IX_Teams_ModifiedById",
                table: "Teams",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamName",
                table: "Teams",
                column: "TeamName",
                unique: true);

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
                name: "IX_UserDocuments_UserId_FileName",
                table: "UserDocuments",
                columns: new[] { "UserId", "FileName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupName",
                table: "UserGroups",
                column: "GroupName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_ModifiedById",
                table: "UserGroups",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ModifiedById",
                table: "UserRoles",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_JobId",
                table: "Users",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModifiedById",
                table: "Users",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedEmail",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedUsername",
                table: "Users",
                column: "NormalizedUsername",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PayrollLocationId",
                table: "Users",
                column: "PayrollLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReportsToId",
                table: "Users",
                column: "ReportsToId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamId",
                table: "Users",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserGroupId",
                table: "Users",
                column: "UserGroupId");

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
                name: "FK_Banks_Users_ModifiedById",
                table: "Banks",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Departments_DepartmentId",
                table: "Bids",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_DzdStatuses_DzdStatusId",
                table: "Bids",
                column: "DzdStatusId",
                principalTable: "DzdStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Periods_PeriodId",
                table: "Bids",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Salesforces_IntertechTeamId",
                table: "Bids",
                column: "IntertechTeamId",
                principalTable: "Salesforces",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Teams_TeamId",
                table: "Bids",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Users_AnalystId",
                table: "Bids",
                column: "AnalystId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Users_DeveloperId",
                table: "Bids",
                column: "DeveloperId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Users_ModifiedById",
                table: "Bids",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Users_ProjectManagerId",
                table: "Bids",
                column: "ProjectManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CareerMapRules_Jobs_JobId",
                table: "CareerMapRules",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CareerMapRules_Users_ModifiedById",
                table: "CareerMapRules",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DefaultDocuments_Users_ModifiedById",
                table: "DefaultDocuments",
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
                name: "FK_DzdStatuses_Users_ModifiedById",
                table: "DzdStatuses",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExCompanyHistories_Users_ModifiedById",
                table: "ExCompanyHistories",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExCompanyHistories_Users_UserId",
                table: "ExCompanyHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeadLeadCoefficients_Jobs_JobId",
                table: "HeadLeadCoefficients",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeadLeadCoefficients_Periods_PeriodId",
                table: "HeadLeadCoefficients",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HeadLeadCoefficients_Users_ModifiedById",
                table: "HeadLeadCoefficients",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuePaymentTypes_Periods_PeriodId",
                table: "IssuePaymentTypes",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuePaymentTypes_Users_ModifiedById",
                table: "IssuePaymentTypes",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuePriorities_Users_ModifiedById",
                table: "IssuePriorities",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssueStatuses_Users_ModifiedById",
                table: "IssueStatuses",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssueTypes_Users_ModifiedById",
                table: "IssueTypes",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Itsms_Periods_PeriodId",
                table: "Itsms",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Itsms_Resolutions_ResolutionId",
                table: "Itsms",
                column: "ResolutionId",
                principalTable: "Resolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Itsms_Teams_TeamId",
                table: "Itsms",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Itsms_Users_AsigneeId",
                table: "Itsms",
                column: "AsigneeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Itsms_Users_ModifiedById",
                table: "Itsms",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JiraStatuses_Users_ModifiedById",
                table: "JiraStatuses",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Users_ModifiedById",
                table: "Jobs",
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
                name: "FK_Periods_Users_ModifiedById",
                table: "Periods",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBonusCoefficients_Users_ModifiedById",
                table: "ProjectBonusCoefficients",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDocuments_Projects_ProjectId",
                table: "ProjectDocuments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDocuments_Users_ModifiedById",
                table: "ProjectDocuments",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectInvoices_Users_ModifiedById",
                table: "ProjectInvoices",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Salesforces_IntertechTeamId",
                table: "Projects",
                column: "IntertechTeamId",
                principalTable: "Salesforces",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Teams_TeamId",
                table: "Projects",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_AnalystId",
                table: "Projects",
                column: "AnalystId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_DeveloperId",
                table: "Projects",
                column: "DeveloperId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_ModifiedById",
                table: "Projects",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resolutions_Users_ModifiedById",
                table: "Resolutions",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_ModifiedById",
                table: "Roles",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Salesforces_Users_ModifiedById",
                table: "Salesforces",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TargetEfforts_Users_ModifiedById",
                table: "TargetEfforts",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TargetEfforts_Users_UserId",
                table: "TargetEfforts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Users_ModifiedById",
                table: "Teams",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Users_ModifiedById",
                table: "Trainings",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDocumentCategories_Users_ModifiedById",
                table: "UserDocumentCategories",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDocuments_Users_ModifiedById",
                table: "UserDocuments",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDocuments_Users_UserId",
                table: "UserDocuments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Users_ModifiedById",
                table: "UserGroups",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_ModifiedById",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Users_ModifiedById",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollLocations_Users_ModifiedById",
                table: "PayrollLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Users_ModifiedById",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Users_ModifiedById",
                table: "UserGroups");

            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "CareerMapRules");

            migrationBuilder.DropTable(
                name: "DefaultDocuments");

            migrationBuilder.DropTable(
                name: "ExCompanyHistories");

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
                name: "UserDocuments");

            migrationBuilder.DropTable(
                name: "UserRoles");

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
                name: "UserDocumentCategories");

            migrationBuilder.DropTable(
                name: "Roles");

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

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "PayrollLocations");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "UserGroups");
        }
    }
}
