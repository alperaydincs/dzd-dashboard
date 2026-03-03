namespace DZDDashboard.Data.Entities;

public class Job : IAuditableEntity
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public List<User>? Users { get; set; }
    public List<HeadLeadCoefficient>? HeadLeadCoefficients { get; set; }
    public int? Level { get; set; }
    public DateTime? ModifiedAt { get; set; }   
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
