using DZDDashboard.Common.DTOs.Users;

namespace DZDDashboard.Common.DTOs
{
    public record ProjectDto
    {
        public int Id { get; init; }
        public int? BankId { get; init; }
        public BankDto? Bank { get; init; }
        public string? JiraProjectNo { get; init; }
        public string? JiraTaskNo { get; init; }
        public string? ProjectName { get; init; }
        public int? DzdStatusId { get; init; }
        public DzdStatusDto? DzdStatus { get; init; }
        public int? JiraStatusId { get; init; }
        public JiraStatusDto? JiraStatus { get; init; }
        public int? DepartmentId { get; init; }
        public DepartmentDto? Department { get; init; }
        public int? TeamId { get; init; }
        public TeamDto? Team { get; init; }
        public decimal TotalEffort { get; init; }
        public int? DeveloperId { get; init; }
        public UserDto? Developer { get; init; }
        public decimal DeveloperEffort { get; init; }
        public int? AnalystId { get; init; }
        public UserDto? Analyst { get; init; }
        public decimal AnalystEffort { get; init; }
        public int? ProjectManagerId { get; init; }
        public UserDto? ProjectManager { get; init; }
        public decimal ProjectManagerEffort { get; init; }
        public int? PeriodId { get; init; }
        public PeriodDto? Period { get; init; }
        public string? Notes { get; init; }
        public int? IntertechTeamId { get; init; }
        public SalesforceDto? IntertechTeam { get; init; }
        public string? Color { get; init; }
        public bool IsIncludedInBonus { get; init; }
    }
}
