namespace DZDDashboard.Common.DTOs;

// Validation is handled exclusively by UpdateCitizenshipInfoDtoValidator (FluentValidation).
// Data annotations removed to avoid dual-validation with different error formats.
public record UpdateCitizenshipInfoDto
{
    public DateTime? DateOfBirth       { get; init; }
    public string?   Gender            { get; init; }
    public string?   Nationality       { get; init; }
    public string?   CitizenshipNumber { get; init; }
    public bool      DisabilityStatus  { get; init; }
    public string?   DisabilityDegree  { get; init; }
}
