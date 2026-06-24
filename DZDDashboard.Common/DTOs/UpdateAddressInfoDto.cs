namespace DZDDashboard.Common.DTOs;

public record UpdateAddressInfoDto
{
    public string? LegalAddress        { get; init; }
    public string? LegalAddressCity    { get; init; }
    public string? LegalAddressCountry { get; init; }
    public string? CurrentAddress      { get; init; }
    public string? City                { get; init; }
    public string? Country             { get; init; }
}
