namespace DZDDashboard.Data.Entities;

public class EducationHistory : AuditableEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public int? EducationLevelId { get; set; }
    public EducationLevelEntity? EducationLevelRef { get; set; }
    public string? Institution { get; set; }
    public string? Program { get; set; }
    public DateTime? GraduationDate { get; set; }
    public string? Status { get; set; }
}