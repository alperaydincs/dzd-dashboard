using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    public partial class MakeUserSlugRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE Users
SET Slug = CASE
    WHEN LTRIM(RTRIM(ISNULL(FirstName,'') + '-' + ISNULL(LastName,''))) IN ('', '-')
        THEN 'user-' + CAST(Id AS nvarchar(20))
    ELSE LOWER(
        REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
            LTRIM(RTRIM(ISNULL(FirstName,'') + '-' + ISNULL(LastName,'')))
        , ' ', '-'), N'ç', 'c'), N'Ç', 'c'), N'ğ', 'g'), N'Ğ', 'g'), N'ı', 'i'), N'İ', 'i'),
            N'ş', 's'), N'Ş', 's'), N'ö', 'o'), N'Ö', 'o'), N'ü', 'u'), N'Ü', 'u')
    )
END
WHERE Slug IS NULL OR Slug = '';");

            migrationBuilder.DropIndex(
                name: "IX_Users_Slug",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Slug",
                table: "Users",
                column: "Slug",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Slug",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Slug",
                table: "Users",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");
        }
    }
}
