namespace DZDDashboard.Data.Entities;

public class ChecklistStepTemplate : AuditableEntity
{
    public int Id { get; set; }

    public int ProcessTemplateId { get; set; }
    public ProcessTemplate? ProcessTemplate { get; set; }

    public string Title    { get; set; } = string.Empty;
    public int    Sequence { get; set; }

    public bool IsRequired { get; set; } = true;
}
