using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class UnifyAdditionalPaymentAndDeductionDateRanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "UserDeductions",
                type: "datetime2",
                nullable: true);

            // Backfill StartDate from the old one-time PaymentDate before it's dropped, so no existing
            // additional payment loses its date.
            migrationBuilder.Sql(
                "UPDATE [UserAdditionalPayments] SET [StartDate] = [PaymentDate] WHERE [StartDate] IS NULL AND [PaymentDate] IS NOT NULL;");
            migrationBuilder.Sql(
                "UPDATE [UserAdditionalPayments] SET [StartDate] = [CreatedAt] WHERE [StartDate] IS NULL;");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "UserAdditionalPayments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "UserAdditionalPayments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "UserDeductions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "UserAdditionalPayments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "UserAdditionalPayments",
                type: "datetime2",
                nullable: true);
        }
    }
}
