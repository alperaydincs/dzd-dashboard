using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedDtoBackedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPositionHistories_Departments_DepartmentId",
                table: "UserPositionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPositionHistories_Teams_TeamId",
                table: "UserPositionHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserPositionHistories_DepartmentId",
                table: "UserPositionHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserPositionHistories_TeamId",
                table: "UserPositionHistories");

            migrationBuilder.DropColumn(
                name: "GrossAmount",
                table: "UserSalaryHistories");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "UserPositionHistories");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "UserPositionHistories");

            migrationBuilder.DropColumn(
                name: "Payer",
                table: "UserBenefitRecords");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "UserBenefitRecords");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "UserBenefitRecords");

            migrationBuilder.RenameColumn(
                name: "NetAmount",
                table: "UserSalaryHistories",
                newName: "Amount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "UserSalaryHistories",
                newName: "NetAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "GrossAmount",
                table: "UserSalaryHistories",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "UserPositionHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "UserPositionHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payer",
                table: "UserBenefitRecords",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceId",
                table: "UserBenefitRecords",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "UserBenefitRecords",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserPositionHistories_DepartmentId",
                table: "UserPositionHistories",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPositionHistories_TeamId",
                table: "UserPositionHistories",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositionHistories_Departments_DepartmentId",
                table: "UserPositionHistories",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositionHistories_Teams_TeamId",
                table: "UserPositionHistories",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
