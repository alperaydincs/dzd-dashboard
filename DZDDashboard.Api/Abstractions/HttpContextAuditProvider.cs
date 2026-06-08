using DZDDashboard.Common.Constants;
using DZDDashboard.Data.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DZDDashboard.Api.Abstractions;

/// <summary>
/// Reads the current user-id from the HTTP request's claims.
/// Lives in the Api layer because it depends on <see cref="IHttpContextAccessor"/>,
/// an ASP.NET Core hosting concern that must not leak into the Data layer.
/// </summary>
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
