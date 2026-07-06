namespace DZDDashboard.Data.Entities;

public class DocumentTemplate : AuditableEntity
{
    public int Id { get; set; }
    public int ProcessTemplateId { get; set; }
    public ProcessTemplate? ProcessTemplate { get; set; }
    public string Name     { get; set; } = string.Empty;
    public int    Sequence { get; set; }
    public bool IsRequired   { get; set; } = true;
    public int  DeadlineDays { get; set; }
}
