namespace DZDDashboard.Common.DTOs;

public record UpdateContactInfoDto
{
    public string? WorkPhoneNumber     { get; set; }
    public string? PersonalEmail       { get; set; }
    public string? PersonalPhoneNumber { get; set; }
}
