namespace DZDDashboard.Data.Abstractions;

public interface IAuditProvider
{
    DateTime GetNow();
    int? GetCurrentUserId();
}
