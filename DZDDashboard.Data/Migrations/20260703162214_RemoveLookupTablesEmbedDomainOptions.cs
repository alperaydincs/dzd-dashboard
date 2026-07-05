using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLookupTablesEmbedDomainOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add the new code columns.
            migrationBuilder.AddColumn<string>(name: "ContractType",   table: "Users",                  type: "nvarchar(200)", maxLength: 200, nullable: true);
            migrationBuilder.AddColumn<string>(name: "WorkModel",      table: "Users",                  type: "nvarchar(200)", maxLength: 200, nullable: true);
            migrationBuilder.AddColumn<string>(name: "EducationLevel", table: "UserEducationHistories", type: "nvarchar(200)", maxLength: 200, nullable: true);
            migrationBuilder.AddColumn<string>(name: "DeductionType",  table: "UserDeductions",         type: "nvarchar(200)", maxLength: 200, nullable: true);
            migrationBuilder.AddColumn<string>(name: "RelationType",   table: "UserBenefitDependents",  type: "nvarchar(200)", maxLength: 200, nullable: true);
            migrationBuilder.AddColumn<string>(name: "PaymentType",    table: "UserAdditionalPayments", type: "nvarchar(200)", maxLength: 200, nullable: true);
            migrationBuilder.AddColumn<string>(name: "RelationType",   table: "ChecklistItemDependents",type: "nvarchar(200)", maxLength: 200, nullable: true);

            // 2. Backfill the codes from the lookup tables while they still exist.
            migrationBuilder.Sql("UPDATE u SET u.ContractType = t.Name FROM [Users] u INNER JOIN [ContractTypes] t ON u.ContractTypeId = t.Id;");
            migrationBuilder.Sql("UPDATE u SET u.WorkModel = t.Name FROM [Users] u INNER JOIN [WorkModels] t ON u.WorkModelId = t.Id;");
            migrationBuilder.Sql("UPDATE e SET e.EducationLevel = t.Name FROM [UserEducationHistories] e INNER JOIN [EducationLevels] t ON e.EducationLevelId = t.Id;");
            migrationBuilder.Sql("UPDATE d SET d.DeductionType = t.Name FROM [UserDeductions] d INNER JOIN [DeductionTypes] t ON d.DeductionTypeId = t.Id;");
            migrationBuilder.Sql("UPDATE b SET b.RelationType = t.Name FROM [UserBenefitDependents] b INNER JOIN [DependentTypes] t ON b.DependentTypeId = t.Id;");
            migrationBuilder.Sql("UPDATE p SET p.PaymentType = t.Name FROM [UserAdditionalPayments] p INNER JOIN [AdditionalPaymentTypes] t ON p.PaymentTypeId = t.Id;");
            migrationBuilder.Sql("UPDATE c SET c.RelationType = t.Name FROM [ChecklistItemDependents] c INNER JOIN [DependentTypes] t ON c.DependentTypeId = t.Id;");

            // 3. Drop the old foreign keys, indexes, columns and lookup tables.
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItemDependents_DependentTypes_DependentTypeId",
                table: "ChecklistItemDependents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAdditionalPayments_AdditionalPaymentTypes_PaymentTypeId",
                table: "UserAdditionalPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBenefitDependents_DependentTypes_DependentTypeId",
                table: "UserBenefitDependents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDeductions_DeductionTypes_DeductionTypeId",
                table: "UserDeductions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEducationHistories_EducationLevels_EducationLevelId",
                table: "UserEducationHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ContractTypes_ContractTypeId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_WorkModels_WorkModelId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AdditionalPaymentTypes");

            migrationBuilder.DropTable(
                name: "ContractTypes");

            migrationBuilder.DropTable(
                name: "DeductionTypes");

            migrationBuilder.DropTable(
                name: "DependentTypes");

            migrationBuilder.DropTable(
                name: "EducationLevels");

            migrationBuilder.DropTable(
                name: "WorkModels");

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
                name: "IX_UserDeductions_DeductionTypeId",
                table: "UserDeductions");

            migrationBuilder.DropIndex(
                name: "IX_UserBenefitDependents_DependentTypeId",
                table: "UserBenefitDependents");

            migrationBuilder.DropIndex(
                name: "IX_UserAdditionalPayments_PaymentTypeId",
                table: "UserAdditionalPayments");

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
                name: "DeductionTypeId",
                table: "UserDeductions");

            migrationBuilder.DropColumn(
                name: "DependentTypeId",
                table: "UserBenefitDependents");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "UserAdditionalPayments");

            migrationBuilder.DropColumn(
                name: "DependentTypeId",
                table: "ChecklistItemDependents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "DeductionTypeId",
                table: "UserDeductions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DependentTypeId",
                table: "UserBenefitDependents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeId",
                table: "UserAdditionalPayments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DependentTypeId",
                table: "ChecklistItemDependents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdditionalPaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalPaymentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalPaymentTypes_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractTypes_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeductionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeductionTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeductionTypes_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DependentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DependentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DependentTypes_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EducationLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationLevels_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkModels_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AdditionalPaymentTypes",
                columns: new[] { "Id", "CreatedAt", "ModifiedAt", "ModifiedById", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Premium" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Bonus" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advance" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Overtime" },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Other" }
                });

            migrationBuilder.InsertData(
                table: "ContractTypes",
                columns: new[] { "Id", "CreatedAt", "ModifiedAt", "ModifiedById", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Full-time" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Part-time" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Contract" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Temporary" }
                });

            migrationBuilder.InsertData(
                table: "DeductionTypes",
                columns: new[] { "Id", "CreatedAt", "ModifiedAt", "ModifiedById", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Social Security" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Garnishment" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advance" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Tax" },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Other" }
                });

            migrationBuilder.InsertData(
                table: "DependentTypes",
                columns: new[] { "Id", "CreatedAt", "ModifiedAt", "ModifiedById", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Spouse" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Child" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Other" }
                });

            migrationBuilder.InsertData(
                table: "EducationLevels",
                columns: new[] { "Id", "CreatedAt", "ModifiedAt", "ModifiedById", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "High School" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Associate" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Bachelor's Degree" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Master's Degree" },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "PhD" },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Other" }
                });

            migrationBuilder.InsertData(
                table: "WorkModels",
                columns: new[] { "Id", "CreatedAt", "ModifiedAt", "ModifiedById", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Remote" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hybrid" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "On-site" }
                });

            // Restore the foreign-key ids from the code columns before dropping them.
            migrationBuilder.Sql("UPDATE u SET u.ContractTypeId = t.Id FROM [Users] u INNER JOIN [ContractTypes] t ON u.ContractType = t.Name;");
            migrationBuilder.Sql("UPDATE u SET u.WorkModelId = t.Id FROM [Users] u INNER JOIN [WorkModels] t ON u.WorkModel = t.Name;");
            migrationBuilder.Sql("UPDATE e SET e.EducationLevelId = t.Id FROM [UserEducationHistories] e INNER JOIN [EducationLevels] t ON e.EducationLevel = t.Name;");
            migrationBuilder.Sql("UPDATE d SET d.DeductionTypeId = t.Id FROM [UserDeductions] d INNER JOIN [DeductionTypes] t ON d.DeductionType = t.Name;");
            migrationBuilder.Sql("UPDATE b SET b.DependentTypeId = t.Id FROM [UserBenefitDependents] b INNER JOIN [DependentTypes] t ON b.RelationType = t.Name;");
            migrationBuilder.Sql("UPDATE p SET p.PaymentTypeId = t.Id FROM [UserAdditionalPayments] p INNER JOIN [AdditionalPaymentTypes] t ON p.PaymentType = t.Name;");
            migrationBuilder.Sql("UPDATE c SET c.DependentTypeId = t.Id FROM [ChecklistItemDependents] c INNER JOIN [DependentTypes] t ON c.RelationType = t.Name;");

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
                name: "IX_UserDeductions_DeductionTypeId",
                table: "UserDeductions",
                column: "DeductionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefitDependents_DependentTypeId",
                table: "UserBenefitDependents",
                column: "DependentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalPayments_PaymentTypeId",
                table: "UserAdditionalPayments",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItemDependents_DependentTypeId",
                table: "ChecklistItemDependents",
                column: "DependentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalPaymentTypes_ModifiedById",
                table: "AdditionalPaymentTypes",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalPaymentTypes_Name",
                table: "AdditionalPaymentTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractTypes_ModifiedById",
                table: "ContractTypes",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ContractTypes_Name",
                table: "ContractTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeductionTypes_ModifiedById",
                table: "DeductionTypes",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DeductionTypes_Name",
                table: "DeductionTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DependentTypes_ModifiedById",
                table: "DependentTypes",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DependentTypes_Name",
                table: "DependentTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevels_ModifiedById",
                table: "EducationLevels",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevels_Name",
                table: "EducationLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkModels_ModifiedById",
                table: "WorkModels",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkModels_Name",
                table: "WorkModels",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItemDependents_DependentTypes_DependentTypeId",
                table: "ChecklistItemDependents",
                column: "DependentTypeId",
                principalTable: "DependentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAdditionalPayments_AdditionalPaymentTypes_PaymentTypeId",
                table: "UserAdditionalPayments",
                column: "PaymentTypeId",
                principalTable: "AdditionalPaymentTypes",
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
                name: "FK_UserDeductions_DeductionTypes_DeductionTypeId",
                table: "UserDeductions",
                column: "DeductionTypeId",
                principalTable: "DeductionTypes",
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

            // Finally drop the code columns now that the ids have been restored.
            migrationBuilder.DropColumn(name: "ContractType",   table: "Users");
            migrationBuilder.DropColumn(name: "WorkModel",      table: "Users");
            migrationBuilder.DropColumn(name: "EducationLevel", table: "UserEducationHistories");
            migrationBuilder.DropColumn(name: "DeductionType",  table: "UserDeductions");
            migrationBuilder.DropColumn(name: "RelationType",   table: "UserBenefitDependents");
            migrationBuilder.DropColumn(name: "PaymentType",    table: "UserAdditionalPayments");
            migrationBuilder.DropColumn(name: "RelationType",   table: "ChecklistItemDependents");
        }
    }
}
