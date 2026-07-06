namespace DZDDashboard.Data.Entities;

public class ProcessTemplate : AuditableEntity
{
    public int Id { get; set; }
    public string Kind { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int    Sequence { get; set; }
    public List<ChecklistStepTemplate> ChecklistItems { get; set; } = [];
    public List<DocumentTemplate>      Documents      { get; set; } = [];
}
