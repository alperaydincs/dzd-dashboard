using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildInfo_Users_UserId",
                table: "ChildInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_GradeHistory_Users_UserId",
                table: "GradeHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_SalaryHistory_Users_UserId",
                table: "SalaryHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalaryHistory",
                table: "SalaryHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradeHistory",
                table: "GradeHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChildInfo",
                table: "ChildInfo");

            migrationBuilder.RenameTable(
                name: "SalaryHistory",
                newName: "UserSalaryHistories");

            migrationBuilder.RenameTable(
                name: "GradeHistory",
                newName: "UserGradeHistories");

            migrationBuilder.RenameTable(
                name: "ChildInfo",
                newName: "UserChildren");

            migrationBuilder.RenameIndex(
                name: "IX_SalaryHistory_UserId",
                table: "UserSalaryHistories",
                newName: "IX_UserSalaryHistories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GradeHistory_UserId",
                table: "UserGradeHistories",
                newName: "IX_UserGradeHistories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ChildInfo_UserId",
                table: "UserChildren",
                newName: "IX_UserChildren_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "UserChildren",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSalaryHistories",
                table: "UserSalaryHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGradeHistories",
                table: "UserGradeHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChildren",
                table: "UserChildren",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChildren_Users_UserId",
                table: "UserChildren",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGradeHistories_Users_UserId",
                table: "UserGradeHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSalaryHistories_Users_UserId",
                table: "UserSalaryHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChildren_Users_UserId",
                table: "UserChildren");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGradeHistories_Users_UserId",
                table: "UserGradeHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSalaryHistories_Users_UserId",
                table: "UserSalaryHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSalaryHistories",
                table: "UserSalaryHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGradeHistories",
                table: "UserGradeHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChildren",
                table: "UserChildren");

            migrationBuilder.RenameTable(
                name: "UserSalaryHistories",
                newName: "SalaryHistory");

            migrationBuilder.RenameTable(
                name: "UserGradeHistories",
                newName: "GradeHistory");

            migrationBuilder.RenameTable(
                name: "UserChildren",
                newName: "ChildInfo");

            migrationBuilder.RenameIndex(
                name: "IX_UserSalaryHistories_UserId",
                table: "SalaryHistory",
                newName: "IX_SalaryHistory_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGradeHistories_UserId",
                table: "GradeHistory",
                newName: "IX_GradeHistory_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserChildren_UserId",
                table: "ChildInfo",
                newName: "IX_ChildInfo_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "ChildInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalaryHistory",
                table: "SalaryHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradeHistory",
                table: "GradeHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChildInfo",
                table: "ChildInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildInfo_Users_UserId",
                table: "ChildInfo",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeHistory_Users_UserId",
                table: "GradeHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryHistory_Users_UserId",
                table: "SalaryHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
