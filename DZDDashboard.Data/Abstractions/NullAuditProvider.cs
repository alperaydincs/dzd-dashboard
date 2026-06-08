namespace DZDDashboard.Data.Abstractions;

/// <summary>
/// No-op audit provider used by EF Core migrations and unit tests that construct
/// <see cref="AppDbContext"/> without a real DI container.
/// </summary>
public sealed class NullAuditProvider : IAuditProvider
{
    public static readonly NullAuditProvider Instance = new();

    public DateTime GetNow()           => DateTime.UtcNow;
    public int?     GetCurrentUserId() => null;
}
