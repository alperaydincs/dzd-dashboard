namespace DZDDashboard.Common.Constants;

public static class AvatarConstants
{
    public const long MaxFileSizeBytes = 5 * 1024 * 1024;
    public static readonly IReadOnlyList<string> AllowedMimeTypes =
        ["image/jpeg", "image/png", "image/webp", "image/gif"];

    public static readonly IReadOnlyList<string> AllowedExtensions =
        [".jpg", ".jpeg", ".png", ".webp", ".gif"];
}
