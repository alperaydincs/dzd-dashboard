namespace DZDDashboard.Common.DTOs;

public record OrgChartUserDto
{
    public int     Id        { get; init; }
    public string? Slug      { get; init; }
    public string? FirstName { get; init; }
    public string? LastName  { get; init; }
    public int?    AvatarColorIndex { get; init; }
    public string? Email     { get; init; }
    public JobDto? Job       { get; init; }

    /// <summary>Whether the user has an uploaded avatar. The image is served via the
    /// <c>/avatars/{id}</c> proxy rather than embedded here.</summary>
    public bool      HasAvatar       { get; init; }

    /// <summary>Last time the avatar changed; used as a cache-busting token in the avatar URL.</summary>
    public DateTime? AvatarUpdatedAt { get; init; }
}
