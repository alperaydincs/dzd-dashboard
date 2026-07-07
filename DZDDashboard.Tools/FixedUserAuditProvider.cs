using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Tools;

/// <summary>
/// IAuditProvider for console/script contexts (no HttpContext). The user id is supplied by
/// whoever runs the tool, so ModifiedBy/history rows correctly attribute the manual fix
/// instead of being null or a shared DB role.
/// </summary>
public class FixedUserAuditProvider(int performedByUserId) : IAuditProvider
{
    public DateTime GetNow() => DateTime.UtcNow;
    public int? GetCurrentUserId() => performedByUserId;
}
