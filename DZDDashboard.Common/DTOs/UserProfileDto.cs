namespace DZDDashboard.Common.DTOs;

public record UserProfileDto
{
    public int Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public int? AvatarColorIndex { get; init; }
    public bool HasAvatar { get; init; }
    public int? AvatarId  { get; init; }
}
