using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTemplateIsEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "DocumentTemplates");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "ChecklistStepTemplates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "DocumentTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "ChecklistStepTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 9,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 11,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 12,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 13,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 14,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 15,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "ChecklistStepTemplates",
                keyColumn: "Id",
                keyValue: 16,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 9,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "DocumentTemplates",
                keyColumn: "Id",
                keyValue: 11,
                column: "IsEnabled",
                value: true);
        }
    }
}
