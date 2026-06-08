namespace DZDDashboard.Common.DTOs;

/// <summary>
/// Minimal user projection for org-chart nodes.
/// Intentionally excludes avatar base64 — org charts render initials only,
/// which keeps the GET /api/organization/positions payload lean.
/// </summary>
public record OrgChartUserDto
{
    public int     Id        { get; init; }
    public string? FirstName { get; init; }
    public string? LastName  { get; init; }
    public string? Email     { get; init; }
    public JobDto? Job       { get; init; }
}
