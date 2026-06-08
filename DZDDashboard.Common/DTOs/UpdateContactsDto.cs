namespace DZDDashboard.Common.DTOs;

// Validation is handled exclusively by UpdateContactsDtoValidator (FluentValidation).
// Data annotations removed to avoid dual-validation with different error formats.
public record UpdateContactsDto
{
    public string? Email               { get; init; }
    public string? PhoneNumber         { get; init; }
    public string? PersonalEmail       { get; init; }
    public string? PersonalPhoneNumber { get; init; }
}
