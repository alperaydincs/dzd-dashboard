namespace DZDDashboard.Data.Abstractions;

public sealed class NullAuditProvider : IAuditProvider
{
    public static readonly NullAuditProvider Instance = new();
    public DateTime GetNow()           => DateTime.UtcNow;
    public int?     GetCurrentUserId() => null;
}
