namespace DZDDashboard.Data.Entities;

public class Training : IAuditableEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? InstructorName { get; set; }
    public string? InstructorCompanyDetails { get; set; }
    public string? Details { get; set; }
    public string? Location { get; set; }
    public int? RepeatFrequency { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
    public List<UserTraining>? UserTrainings { get; set; }
}
