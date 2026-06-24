using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    public partial class RemoveTaxableFlagAndDeductionEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "UserDeductions");

            migrationBuilder.DropColumn(
                name: "TaxableFlag",
                table: "UserAdditionalPayments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "UserDeductions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TaxableFlag",
                table: "UserAdditionalPayments",
                type: "bit",
                nullable: true);
        }
    }
}
