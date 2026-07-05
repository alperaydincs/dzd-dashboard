using DZDDashboard.Common.Constants;

namespace DZDDashboard.Data.Entities;

public class OnboardingProcess : AuditableEntity
{
    public int Id { get; set; }

    public int   UserId { get; set; }
    public User? User   { get; set; }

    public DateTime StartDate { get; set; }

    public int?  ManagerId { get; set; }
    public User? Manager   { get; set; }

    public string    Status      { get; set; } = ProcessStatuses.InProgress;
    public DateTime? CompletedAt { get; set; }

    public List<ChecklistItem> Items { get; set; } = [];
}
