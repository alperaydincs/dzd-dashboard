namespace DZDDashboard.Data.Entities;

public class ProjectDocument : IAuditableEntity
{
    public int Id { get; set; }
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
    public string? DocumentName { get; set; }
    public string? ContentType { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
