namespace DZDDashboard.Data.Entities;

public class ChecklistItemDependent
{
    public int Id { get; set; }

    public int ChecklistItemId { get; set; }
    public ChecklistItem? ChecklistItem { get; set; }

    public int Order { get; set; }

    public string? RelationType { get; set; }
    public string? DependentName { get; set; }
    public decimal Amount        { get; set; }
}
