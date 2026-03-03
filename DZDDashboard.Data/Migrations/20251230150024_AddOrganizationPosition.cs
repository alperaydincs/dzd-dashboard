using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationPositionId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrganizationPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationPositions_OrganizationPositions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "OrganizationPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationPositions_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationPositionId",
                table: "Users",
                column: "OrganizationPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPositions_ModifiedById",
                table: "OrganizationPositions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPositions_ParentId",
                table: "OrganizationPositions",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_OrganizationPositions_OrganizationPositionId",
                table: "Users",
                column: "OrganizationPositionId",
                principalTable: "OrganizationPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_OrganizationPositions_OrganizationPositionId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "OrganizationPositions");

            migrationBuilder.DropIndex(
                name: "IX_Users_OrganizationPositionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OrganizationPositionId",
                table: "Users");
        }
    }
}
