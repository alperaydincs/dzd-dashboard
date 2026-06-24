using DZDDashboard.Common.Constants;

namespace DZDDashboard.Data.Entities;

public class ChecklistItem : AuditableEntity
{
    public int Id { get; set; }

    public int? OnboardingProcessId { get; set; }
    public OnboardingProcess? OnboardingProcess { get; set; }

    public int? OffboardingProcessId { get; set; }
    public OffboardingProcess? OffboardingProcess { get; set; }

    public string StepKey  { get; set; } = string.Empty;
    public string Title    { get; set; } = string.Empty;
    public int    Sequence { get; set; }

    public bool IsRequired { get; set; } = true;
    public bool IsGate     { get; set; }

    public string BenefitKind { get; set; } = ChecklistBenefitKinds.None;

    public string Status { get; set; } = ChecklistItemStatuses.Pending;

    public string? Note { get; set; }

    public DateTime? CompletedAt   { get; set; }
    public int?      CompletedById { get; set; }
    public User?     CompletedBy   { get; set; }

    public int?     EvidenceStoredFileId { get; set; }
    public StoredFile? EvidenceStoredFile { get; set; }
    public string?  EvidenceFileName     { get; set; }
    public string?  EvidenceContentType  { get; set; }

    public string?  ProviderName   { get; set; }
    public string?  Currency       { get; set; }
    public decimal? EmployeeAmount { get; set; }
    public decimal? EmployerAmount { get; set; }

    public int? ReflectedBenefitRecordId { get; set; }

    public List<ChecklistItemDependent> Dependents { get; set; } = [];
}
