namespace DZDDashboard.Client.Services;

public static class AvatarUrl
{
    public static string For(int userId, int? version)
        => $"/avatars/{userId}?v={version ?? 0}";


    public static string Mine(int? version)
        => $"/avatars/me?v={version ?? 0}";
}
