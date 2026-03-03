namespace DZDDashboard.Data.Entities;

public class Project : IAuditableEntity
{
    public int Id { get; set; }
    public Bank? Bank { get; set; }
    public int? BankId { get; set; }
    public string? JiraProjectNo { get; set; }
    public string? JiraTaskNo { get; set; }
    public string? ProjectName { get; set; }
    public DzdStatus? DzdStatus { get; set; }
    public int? DzdStatusId { get; set; }
    public JiraStatus? JiraStatus { get; set; }
    public int? JiraStatusId { get; set; }
    public Department? Department { get; set; }
    public int? DepartmentId { get; set; }
    public Team? Team { get; set; }
    public int? TeamId { get; set; }
    public decimal TotalEffort { get; set; }
    public User? Developer { get; set; }
    public int? DeveloperId { get; set; }
    public decimal DeveloperEffort { get; set; }
    public User? Analyst { get; set; }
    public int? AnalystId { get; set; }
    public decimal AnalystEffort { get; set; }
    public User? ProjectManager { get; set; }
    public int? ProjectManagerId { get; set; }
    public decimal ProjectManagerEffort { get; set; }
    public Period? Period { get; set; }
    public int? PeriodId { get; set; }
    public string? Notes { get; set; }
    public Salesforce? IntertechTeam { get; set; }
    public int? IntertechTeamId { get; set; }
    public string? Color { get; set; }
    public bool IsIncludedInBonus { get; set; } = true;
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
