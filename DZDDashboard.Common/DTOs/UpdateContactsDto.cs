namespace DZDDashboard.Common.DTOs;

public record UpdateContactsDto
{
    public string? Email               { get; init; }
    public string? PhoneNumber         { get; init; }
    public string? PersonalEmail       { get; init; }
    public string? PersonalPhoneNumber { get; init; }
}
