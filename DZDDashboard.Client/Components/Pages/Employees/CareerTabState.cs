using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Components.Pages.Employees;

internal sealed class CareerTabState
{
    public bool IsLoaded { get; private set; }

    public List<CompanyDto>    Companies   { get; set; } = [];
    public List<DepartmentDto> AllDepts    { get; set; } = [];
    public List<TeamDto>       AllTeams    { get; set; } = [];
    public List<JobDto>        AllJobs     { get; set; } = [];
    public List<CareerPathDto> CareerPaths { get; set; } = [];

    public string? CompanyName { get; set; }
    public int?    DeptId      { get; set; }
    public int?    TeamId      { get; set; }
    public int?    PathId      { get; set; }
    public int?    JobId       { get; set; }
    public int?    Grade       { get; set; }

    public UserSearchResultDto? SelectedManager { get; set; }

    public List<DepartmentDto> FilteredDepts { get; set; } = [];
    public List<TeamDto>       FilteredTeams { get; set; } = [];
    public List<JobDto>        PathJobs      { get; set; } = [];
    public List<int>           JobGrades     { get; set; } = [];

    public string DeptName => AllDepts.FirstOrDefault(d => d.Id == DeptId)?.Name    ?? "-";
    public string TeamName => AllTeams.FirstOrDefault(t => t.Id == TeamId)?.Name    ?? "-";
    public string PathName => CareerPaths.FirstOrDefault(p => p.Id == PathId)?.Name ?? "-";
    public string JobName  => AllJobs.FirstOrDefault(j => j.Id == JobId)?.Name      ?? "-";


    public void MarkLoaded() => IsLoaded = true;

    public void Invalidate() => IsLoaded = false;
}
