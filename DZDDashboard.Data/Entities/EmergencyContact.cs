namespace DZDDashboard.Data.Entities;

public class EmergencyContact : EntityWithHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public string? FullName { get; set; }
    public string? Relationship { get; set; }
    public string? PhoneNumber { get; set; }
}