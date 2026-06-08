namespace DZDDashboard.Data.Entities;

public class Training : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? InstructorName { get; set; }
    public string? InstructorCompanyDetails { get; set; }
    public string? Details { get; set; }
    public string? Location { get; set; }
    public int? RepeatFrequency { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;    public List<UserTraining>? UserTrainings { get; set; }
}
