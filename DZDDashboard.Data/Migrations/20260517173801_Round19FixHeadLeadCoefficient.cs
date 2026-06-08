using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class Round19FixHeadLeadCoefficient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadLeadCoefficients_Periods_PeriodId1",
                table: "HeadLeadCoefficients");

            migrationBuilder.DropIndex(
                name: "IX_HeadLeadCoefficients_PeriodId1",
                table: "HeadLeadCoefficients");

            migrationBuilder.DropColumn(
                name: "PeriodId1",
                table: "HeadLeadCoefficients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeriodId1",
                table: "HeadLeadCoefficients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeadLeadCoefficients_PeriodId1",
                table: "HeadLeadCoefficients",
                column: "PeriodId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadLeadCoefficients_Periods_PeriodId1",
                table: "HeadLeadCoefficients",
                column: "PeriodId1",
                principalTable: "Periods",
                principalColumn: "Id");
        }
    }
}
