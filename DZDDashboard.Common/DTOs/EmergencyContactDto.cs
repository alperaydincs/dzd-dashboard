namespace DZDDashboard.Common.DTOs;

public record EmergencyContactDto
{
    public int     Id           { get; set; }
    public string? FullName     { get; set; }
    public string? Relationship { get; set; }
    public string? PhoneNumber  { get; set; }
}
