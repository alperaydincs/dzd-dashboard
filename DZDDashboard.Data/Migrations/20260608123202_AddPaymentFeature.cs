using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    public partial class AddPaymentFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSalaryHistories_UserId",
                table: "UserSalaryHistories");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "UserSalaryHistories",
                newName: "NetAmount");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserSalaryHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "UserSalaryHistories",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "GrossAmount",
                table: "UserSalaryHistories",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserSalaryHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserSalaryHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "UserSalaryHistories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayrollCycle",
                table: "UserSalaryHistories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Period",
                table: "UserSalaryHistories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserAdditionalPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Period = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaxableFlag = table.Column<bool>(type: "bit", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BenefitType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Period = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProviderName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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
                name: "UserBenefitDependents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    DependentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BenefitRecordId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_UserSalaryHistories_ModifiedById",
                table: "UserSalaryHistories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserSalaryHistories_UserId_StartDate",
                table: "UserSalaryHistories",
                columns: new[] { "UserId", "StartDate" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserSalaryHistories_Users_ModifiedById",
                table: "UserSalaryHistories",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSalaryHistories_Users_ModifiedById",
                table: "UserSalaryHistories");

            migrationBuilder.DropTable(
                name: "UserAdditionalPayments");

            migrationBuilder.DropTable(
                name: "UserBenefitDependents");

            migrationBuilder.DropTable(
                name: "UserBenefitRecords");

            migrationBuilder.DropIndex(
                name: "IX_UserSalaryHistories_ModifiedById",
                table: "UserSalaryHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserSalaryHistories_UserId_StartDate",
                table: "UserSalaryHistories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserSalaryHistories");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "UserSalaryHistories");

            migrationBuilder.DropColumn(
                name: "GrossAmount",
                table: "UserSalaryHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserSalaryHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserSalaryHistories");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "UserSalaryHistories");

            migrationBuilder.DropColumn(
                name: "PayrollCycle",
                table: "UserSalaryHistories");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "UserSalaryHistories");

            migrationBuilder.RenameColumn(
                name: "NetAmount",
                table: "UserSalaryHistories",
                newName: "Amount");

            migrationBuilder.CreateIndex(
                name: "IX_UserSalaryHistories_UserId",
                table: "UserSalaryHistories",
                column: "UserId");
        }
    }
}
