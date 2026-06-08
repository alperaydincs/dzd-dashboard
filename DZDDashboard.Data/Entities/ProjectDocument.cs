namespace DZDDashboard.Data.Entities;

public class ProjectDocument : AuditableEntity
{
    public int Id { get; set; }
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
    public string? DocumentName { get; set; }
    public string? ContentType { get; set; }
}