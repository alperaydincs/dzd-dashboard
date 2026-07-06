namespace DZDDashboard.Common.DTOs;

public record EmployeeSensitiveInfoDto
{
    public int     UserId             { get; init; }
    public DateTime? DateOfBirth      { get; init; }
    public string?   Gender           { get; init; }
    public string?   Nationality      { get; init; }
    public string?   CitizenshipNumber { get; init; }
    public bool    DisabilityStatus   { get; init; }
    public string? DisabilityDegree   { get; init; }
    public string? MaritalStatus      { get; init; }
    public string? SpouseFullName     { get; init; }
    public List<ChildInfoDto> Children { get; init; } = [];
    public string? PersonalEmail       { get; init; }
    public string? PersonalPhoneNumber { get; init; }
    public string? LegalAddress        { get; init; }
    public string? LegalAddressCity    { get; init; }
    public string? LegalAddressCountry { get; init; }
    public string? CurrentAddress      { get; init; }
    public DateTime? CurrentAddressChangedAt { get; init; }
    public string? City                { get; init; }
    public string? Country             { get; init; }
}
