namespace DZDDashboard.Data.Entities;

public class Job : AuditableEntity
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int? Level { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<HeadLeadCoefficient> HeadLeadCoefficients { get; set; } = new List<HeadLeadCoefficient>();
}