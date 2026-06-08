namespace DZDDashboard.Data.Entities;

/// <summary>
/// Owned value object for a role/experience duration constraint.
/// Stored as two nullable columns on the owning table. The business rule
/// that only one of Months/Years may be set is enforced at the service layer
/// via FluentValidation (not the DB schema, to keep the schema flexible).
/// </summary>
public class RoleDuration
{
    public int? Months { get; set; }
    public int? Years  { get; set; }

    public bool IsEmpty => !Months.HasValue && !Years.HasValue;
}
