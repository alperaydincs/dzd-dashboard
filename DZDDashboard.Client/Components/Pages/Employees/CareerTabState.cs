using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Components.Pages.Employees;

/// <summary>
/// Encapsulates all mutable state belonging to the Career tab in EmployeeCard.
/// Extracted from EmployeeCard.razor.cs to reduce class size (SRP).
/// </summary>
internal sealed class CareerTabState
{
    public bool IsLoaded  { get; private set; }
    public bool IsEditing { get; private set; }

    // Reference data loaded once when the tab is first opened
    public List<CompanyDto>    Companies   { get; set; } = [];
    public List<DepartmentDto> AllDepts    { get; set; } = [];
    public List<TeamDto>       AllTeams    { get; set; } = [];
    public List<JobDto>        AllJobs     { get; set; } = [];
    public List<CareerPathDto> CareerPaths { get; set; } = [];

    // Currently selected values
    public string? CompanyName { get; set; }
    public int?    DeptId      { get; set; }
    public int?    TeamId      { get; set; }
    public int?    PathId      { get; set; }
    public int?    JobId       { get; set; }
    public int?    Grade       { get; set; }

    // Derived filtered lists (re-computed when parent selection changes)
    public List<DepartmentDto> FilteredDepts { get; set; } = [];
    public List<TeamDto>       FilteredTeams { get; set; } = [];
    public List<JobDto>        PathJobs      { get; set; } = [];
    public List<int>           JobGrades     { get; set; } = [];

    // Display-name helpers
    public string DeptName => AllDepts.FirstOrDefault(d => d.Id == DeptId)?.Name    ?? "-";
    public string TeamName => AllTeams.FirstOrDefault(t => t.Id == TeamId)?.Name    ?? "-";
    public string PathName => CareerPaths.FirstOrDefault(p => p.Id == PathId)?.Name ?? "-";
    public string JobName  => AllJobs.FirstOrDefault(j => j.Id == JobId)?.Name      ?? "-";

    // ── Behaviour methods (mutation kept inside class) ────────────────────────

    /// <summary>Marks reference data as loaded and enters view mode.</summary>
    public void MarkLoaded()
    {
        IsLoaded  = true;
        IsEditing = false;
    }

    /// <summary>Enters edit mode.</summary>
    public void BeginEdit() => IsEditing = true;

    /// <summary>Exits edit mode without clearing loaded reference data.</summary>
    public void CancelEdit() => IsEditing = false;

    /// <summary>
    /// Invalidates loaded data so next tab-open reloads from API.
    /// Call after a successful save that changes the profile.
    /// </summary>
    public void Invalidate()
    {
        IsLoaded  = false;
        IsEditing = false;
    }
}
