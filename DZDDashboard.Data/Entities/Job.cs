namespace DZDDashboard.Data.Entities;

public class Job : EntityWithHistory
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int? Level { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}