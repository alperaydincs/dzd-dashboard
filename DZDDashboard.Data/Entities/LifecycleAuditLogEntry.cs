namespace DZDDashboard.Data.Entities;

public class LifecycleAuditLogEntry
{
    public int Id { get; set; }

    public int? OnboardingProcessId { get; set; }
    public OnboardingProcess? OnboardingProcess { get; set; }

    public int? OffboardingProcessId { get; set; }
    public OffboardingProcess? OffboardingProcess { get; set; }

    public string Action { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;

    public int?  PerformedById { get; set; }
    public User? PerformedBy   { get; set; }

    public DateTime CreatedAt { get; set; }
}
