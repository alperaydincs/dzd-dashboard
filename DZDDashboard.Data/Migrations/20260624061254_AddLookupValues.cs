using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLookupValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LookupValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LookupValues_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "LookupValues",
                columns: new[] { "Id", "Category", "CreatedAt", "IsActive", "ModifiedAt", "ModifiedById", "Sequence", "Value" },
                values: new object[,]
                {
                    { 1, "AdditionalPaymentType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, "Premium" },
                    { 2, "AdditionalPaymentType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 2, "Bonus" },
                    { 3, "AdditionalPaymentType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 3, "Advance" },
                    { 4, "AdditionalPaymentType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 4, "Overtime" },
                    { 5, "AdditionalPaymentType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 5, "Other" },
                    { 6, "DeductionType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, "Social Security" },
                    { 7, "DeductionType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 2, "Garnishment" },
                    { 8, "DeductionType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 3, "Advance" },
                    { 9, "DeductionType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 4, "Tax" },
                    { 10, "DeductionType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 5, "Other" },
                    { 11, "ContractType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, "Full-time" },
                    { 12, "ContractType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 2, "Part-time" },
                    { 13, "ContractType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 3, "Contract" },
                    { 14, "ContractType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 4, "Temporary" },
                    { 15, "WorkModel", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, "Remote" },
                    { 16, "WorkModel", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 2, "Hybrid" },
                    { 17, "WorkModel", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 3, "On-site" },
                    { 18, "EducationLevel", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, "High School" },
                    { 19, "EducationLevel", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 2, "Associate" },
                    { 20, "EducationLevel", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 3, "Bachelor's Degree" },
                    { 21, "EducationLevel", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 4, "Master's Degree" },
                    { 22, "EducationLevel", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 5, "PhD" },
                    { 23, "EducationLevel", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 6, "Other" },
                    { 24, "DependentType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 1, "Spouse" },
                    { 25, "DependentType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 2, "Child" },
                    { 26, "DependentType", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, 3, "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LookupValues_Category_Value",
                table: "LookupValues",
                columns: new[] { "Category", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LookupValues_ModifiedById",
                table: "LookupValues",
                column: "ModifiedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LookupValues");
        }
    }
}
