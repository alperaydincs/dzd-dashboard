namespace DZDDashboard.Data.Entities;

public class ExCompanyHistory : AuditableEntity
{
    public int Id { get; set; }
    public string? CompanyName { get; set; }
    public string? JobTitle { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
}