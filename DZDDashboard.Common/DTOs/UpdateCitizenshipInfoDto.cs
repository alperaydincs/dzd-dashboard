namespace DZDDashboard.Common.DTOs;

public record UpdateCitizenshipInfoDto
{
    public DateTime? DateOfBirth       { get; init; }
    public string?   Gender            { get; init; }
    public string?   Nationality       { get; init; }
    public string?   CitizenshipNumber { get; init; }
    public bool      DisabilityStatus  { get; init; }
    public string?   DisabilityDegree  { get; init; }
}
