using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompanyDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Companies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Companies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
