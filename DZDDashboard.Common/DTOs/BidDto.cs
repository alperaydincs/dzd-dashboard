using DZDDashboard.Common.DTOs.Users;

namespace DZDDashboard.Common.DTOs
{
    public record BidDto
    {
        public int Id { get; init; }
        public string? JiraProjectNo { get; init; }
        public string? ProjectName { get; init; }
        public int? DzdStatusId { get; init; }
        public DzdStatusDto? DzdStatus { get; init; }
        public int? DepartmentId { get; init; }
        public DepartmentDto? Department { get; init; }
        public int? TeamId { get; init; }
        public TeamDto? Team { get; init; }
        public int? PeriodId { get; init; }
        public PeriodDto? Period { get; init; }
        public int? ProjectManagerId { get; init; }
        public UserDto? ProjectManager { get; init; }
        public int? AnalystId { get; init; }
        public UserDto? Analyst { get; init; }
        public int? DeveloperId { get; init; }
        public UserDto? Developer { get; init; }
        public DateTime? CloseDate { get; init; }
        public DateTime? AnalysisDate { get; init; }
        public DateTime? UatDate { get; init; }
        public string? TshirtSize { get; init; }
        public decimal Budget { get; init; }
        public string? Notes { get; init; }
    }
}
