using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class FkPaymentDeductionTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeductionType",
                table: "UserDeductions");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "UserAdditionalPayments");

            migrationBuilder.AddColumn<int>(
                name: "DeductionTypeId",
                table: "UserDeductions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeId",
                table: "UserAdditionalPayments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDeductions_DeductionTypeId",
                table: "UserDeductions",
                column: "DeductionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalPayments_PaymentTypeId",
                table: "UserAdditionalPayments",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAdditionalPayments_AdditionalPaymentTypes_PaymentTypeId",
                table: "UserAdditionalPayments",
                column: "PaymentTypeId",
                principalTable: "AdditionalPaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeductions_DeductionTypes_DeductionTypeId",
                table: "UserDeductions",
                column: "DeductionTypeId",
                principalTable: "DeductionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAdditionalPayments_AdditionalPaymentTypes_PaymentTypeId",
                table: "UserAdditionalPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDeductions_DeductionTypes_DeductionTypeId",
                table: "UserDeductions");

            migrationBuilder.DropIndex(
                name: "IX_UserDeductions_DeductionTypeId",
                table: "UserDeductions");

            migrationBuilder.DropIndex(
                name: "IX_UserAdditionalPayments_PaymentTypeId",
                table: "UserAdditionalPayments");

            migrationBuilder.DropColumn(
                name: "DeductionTypeId",
                table: "UserDeductions");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "UserAdditionalPayments");

            migrationBuilder.AddColumn<string>(
                name: "DeductionType",
                table: "UserDeductions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "UserAdditionalPayments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
