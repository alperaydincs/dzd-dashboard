namespace DZDDashboard.Common.DTOs;

public record UserAvatarDto
{
    public string? ContentBase64 { get; set; }
    public string? ContentType   { get; set; }
}
