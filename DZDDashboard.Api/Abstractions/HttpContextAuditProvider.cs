using DZDDashboard.Common.Constants;
using DZDDashboard.Data.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DZDDashboard.Api.Abstractions;

public class HttpContextAuditProvider(IHttpContextAccessor httpContextAccessor) : IAuditProvider
{
    public DateTime GetNow() => DateTime.UtcNow;

    public int? GetCurrentUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user is null) return null;

        var raw = user.FindFirst(DzdClaimTypes.DatabaseUserId)?.Value
               ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.TryParse(raw, out var id) ? id : null;
    }
}
