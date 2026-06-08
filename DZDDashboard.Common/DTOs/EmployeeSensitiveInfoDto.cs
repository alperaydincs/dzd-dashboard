namespace DZDDashboard.Common.DTOs;

/// <summary>
/// Employee PII fields that require elevated access (SensitiveDataPolicy).
/// Returned only from GET /api/users/{id}/sensitive-info.
/// Non-sensitive work fields remain in <see cref="EmployeeCardDto"/>.
/// </summary>
public record EmployeeSensitiveInfoDto
{
    public int     UserId             { get; init; }

    // ── Identity ──────────────────────────────────────────────────────────────
    public DateTime? DateOfBirth      { get; init; }
    public string?   Gender           { get; init; }
    public string?   Nationality      { get; init; }
    public string?   CitizenshipNumber { get; init; }

    // ── Disability ────────────────────────────────────────────────────────────
    public bool    DisabilityStatus   { get; init; }
    public string? DisabilityDegree   { get; init; }

    // ── Family ────────────────────────────────────────────────────────────────
    public string? MaritalStatus      { get; init; }
    public string? SpouseFullName     { get; init; }
    public List<ChildInfoDto> Children { get; init; } = [];

    // ── Personal contact ──────────────────────────────────────────────────────
    public string? PersonalEmail       { get; init; }
    public string? PersonalPhoneNumber { get; init; }

    // ── Address ───────────────────────────────────────────────────────────────
    public string? LegalAddress        { get; init; }
    public string? CurrentAddress      { get; init; }
    public string? City                { get; init; }
    public string? Country             { get; init; }
}
