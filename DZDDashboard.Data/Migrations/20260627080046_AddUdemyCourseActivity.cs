using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUdemyCourseActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UdemyCourseActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UdemyUserId = table.Column<long>(type: "bigint", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserExternalId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    CourseTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CourseCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CourseDurationMinutes = table.Column<double>(type: "float", nullable: true),
                    CompletionRatio = table.Column<double>(type: "float", nullable: false),
                    EnrollDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAccessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAssigned = table.Column<bool>(type: "bit", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastSyncedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UdemyCourseActivities");
        }
    }
}
