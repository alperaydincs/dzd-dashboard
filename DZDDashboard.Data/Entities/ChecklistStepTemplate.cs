using DZDDashboard.Common.Constants;

namespace DZDDashboard.Data.Entities;

public class ChecklistStepTemplate : AuditableEntity
{
    public int Id { get; set; }

    public string ProcessType { get; set; } = string.Empty;

    public string StepKey  { get; set; } = string.Empty;
    public string Title    { get; set; } = string.Empty;
    public int    Sequence { get; set; }

    public bool IsRequired      { get; set; } = true;
    public bool IsGate          { get; set; }
    public bool RequiresDocument { get; set; }
    public bool IsEnabled       { get; set; } = true;

    public string BenefitKind { get; set; } = ChecklistBenefitKinds.None;
}
