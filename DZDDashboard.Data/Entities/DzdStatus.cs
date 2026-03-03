namespace DZDDashboard.Data.Entities;

public class DzdStatus : IAuditableEntity
{
    public int Id { get; set; }
    public string? DzdStatusName { get; set; }
    public List<Project>? Projects { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
