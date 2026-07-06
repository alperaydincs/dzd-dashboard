using DZDDashboard.Common.Constants;

namespace DZDDashboard.Data.Entities;

public class ChecklistItem : AuditableEntity
{
    public int Id { get; set; }

    public int? OnboardingProcessId { get; set; }
    public OnboardingProcess? OnboardingProcess { get; set; }

    public int? OffboardingProcessId { get; set; }
    public OffboardingProcess? OffboardingProcess { get; set; }

    public string Title    { get; set; } = string.Empty;
    public int    Sequence { get; set; }

    public bool IsRequired { get; set; } = true;

    public string Status { get; set; } = ChecklistItemStatuses.Pending;

    public DateTime? CompletedAt   { get; set; }
    public int?      CompletedById { get; set; }
    public User?     CompletedBy   { get; set; }
}
