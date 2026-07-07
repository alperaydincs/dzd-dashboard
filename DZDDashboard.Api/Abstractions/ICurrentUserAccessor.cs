using DZDDashboard.Common.Constants;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DZDDashboard.Api.Abstractions;

public interface ICurrentUserAccessor
{
    int? UserId { get; }
    int RequiredUserId { get; }
}

public sealed class HttpContextCurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    public int? UserId
    {
        get
        {
            var raw = httpContextAccessor.HttpContext?.User
                .FindFirstValue(DzdClaimTypes.DatabaseUserId);
            return int.TryParse(raw, out var id) ? id : null;
        }
    }

    public int RequiredUserId =>
        UserId ?? throw new UnauthorizedAccessException("Current user id claim is missing.");
}
