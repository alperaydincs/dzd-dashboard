namespace DZDDashboard.Common.DTOs;

// Validation is handled exclusively by UpdateAddressInfoDtoValidator (FluentValidation).
// Data annotations removed to avoid dual-validation with different error formats.
public record UpdateAddressInfoDto
{
    public string? LegalAddress   { get; init; }
    public string? CurrentAddress { get; init; }
    public string? City           { get; init; }
    public string? Country        { get; init; }
}
