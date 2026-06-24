namespace DZDDashboard.Data.Entities;

public class RoleDuration
{
    public int? Months { get; set; }
    public int? Years  { get; set; }

    public bool IsEmpty => !Months.HasValue && !Years.HasValue;
}
