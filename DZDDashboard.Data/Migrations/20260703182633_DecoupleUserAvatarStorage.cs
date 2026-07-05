using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DZDDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class DecoupleUserAvatarStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add StorageId as nullable first so existing rows can be backfilled before the
            // NOT NULL constraint is applied.
            migrationBuilder.AddColumn<int>(
                name: "StorageId",
                table: "UserAvatars",
                type: "int",
                nullable: true);

            // Move each avatar's existing base64 content into StoredFiles (the same table
            // IFileStorageService already uses for documents) and point UserAvatars at it.
            // MERGE (rather than plain INSERT ... SELECT) is required here because it is the
            // only statement that can OUTPUT columns from the *source* row (UserAvatars.Id)
            // alongside the newly inserted StoredFiles.Id, which is what lets us correlate the
            // two tables afterwards without a shared key.
            migrationBuilder.Sql(@"
                DECLARE @Mapping TABLE (AvatarId INT, StorageId INT);

                MERGE StoredFiles AS target
                USING UserAvatars AS source
                ON 1 = 0
                WHEN NOT MATCHED THEN
                    INSERT (Content, ContentType, SizeBytes, CreatedAt)
                    VALUES (
                        CAST(N'' AS XML).value('xs:base64Binary(sql:column(""source.ContentBase64""))', 'varbinary(max)'),
                        source.ContentType,
                        DATALENGTH(CAST(N'' AS XML).value('xs:base64Binary(sql:column(""source.ContentBase64""))', 'varbinary(max)')),
                        GETUTCDATE()
                    )
                OUTPUT source.Id, inserted.Id INTO @Mapping (AvatarId, StorageId);

                UPDATE ua
                SET ua.StorageId = m.StorageId
                FROM UserAvatars ua
                JOIN @Mapping m ON m.AvatarId = ua.Id;
            ");

            migrationBuilder.DropColumn(
                name: "ContentBase64",
                table: "UserAvatars");

            migrationBuilder.AlterColumn<int>(
                name: "StorageId",
                table: "UserAvatars",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_StorageId",
                table: "UserAvatars",
                column: "StorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAvatars_StoredFiles_StorageId",
                table: "UserAvatars",
                column: "StorageId",
                principalTable: "StoredFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAvatars_StoredFiles_StorageId",
                table: "UserAvatars");

            migrationBuilder.DropIndex(
                name: "IX_UserAvatars_StorageId",
                table: "UserAvatars");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "UserAvatars");

            migrationBuilder.AddColumn<string>(
                name: "ContentBase64",
                table: "UserAvatars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
