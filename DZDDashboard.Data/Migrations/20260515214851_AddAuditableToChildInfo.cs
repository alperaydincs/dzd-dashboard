using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    public partial class AddAuditableToChildInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserChildren",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "UserChildren",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "UserChildren",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChildren_ModifiedById",
                table: "UserChildren",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChildren_Users_ModifiedById",
                table: "UserChildren",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChildren_Users_ModifiedById",
                table: "UserChildren");

            migrationBuilder.DropIndex(
                name: "IX_UserChildren_ModifiedById",
                table: "UserChildren");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserChildren");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "UserChildren");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "UserChildren");
        }
    }
}
