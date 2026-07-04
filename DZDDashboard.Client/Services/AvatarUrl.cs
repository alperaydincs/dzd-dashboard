namespace DZDDashboard.Client.Services;

/// <summary>
/// Single source of truth for building avatar image URLs. Today these point at the
/// same-origin <c>/avatars</c> proxy, which streams the image from the API. When avatars
/// move to blob storage, only this helper (and the proxy handler) change — every caller
/// keeps using <see cref="For"/> / <see cref="Mine"/> unchanged.
/// </summary>
/// <remarks>
/// The <c>?v=</c> component is a cache-busting token derived from the avatar's last-modified
/// timestamp. A given URL therefore always maps to one immutable image, so responses can be
/// cached aggressively; when the avatar changes the token (and thus the URL) changes, forcing
/// the browser to fetch the new image.
/// </remarks>
public static class AvatarUrl
{
    /// <summary>Avatar of an arbitrary user (requires the caller to be authorised to view it).</summary>
    public static string For(int userId, DateTime? version)
        => $"/avatars/{userId}?v={version?.Ticks ?? 0}";

    /// <summary>Avatar of the currently signed-in user.</summary>
    public static string Mine(DateTime? version)
        => $"/avatars/me?v={version?.Ticks ?? 0}";
}
