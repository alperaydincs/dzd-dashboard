using DZDDashboard.Common.Constants;
using DZDDashboard.Api.Options;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DZDDashboard.Api.Middleware;

public partial class EntraUserSyncMiddleware(
    RequestDelegate next,
    IDistributedCache cache,
    IOptions<MiddlewareOptions> options,
    ILogger<EntraUserSyncMiddleware> logger)
{
    private readonly TimeSpan _cacheDuration =
        TimeSpan.FromMinutes(options.Value.UserIdCacheDurationMinutes);

    public async Task InvokeAsync(HttpContext context, IUserSyncService userService)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            await next(context);
            return;
        }

        var objectId = GetClaimValue(context.User,
                          DzdClaimTypes.ObjectIdentifier,
                          DzdClaimTypes.ObjectIdentifierShort,
                          DzdClaimTypes.Subject);

        if (string.IsNullOrWhiteSpace(objectId))
        {
            await next(context);
            return;
        }

        if (context.User.Identity is not ClaimsIdentity identity)
        {
            await next(context);
            return;
        }

        if (identity.HasClaim(c => c.Type == DzdClaimTypes.DatabaseUserId))
        {
            await next(context);
            return;
        }

        var cacheKey     = CacheKey(objectId);
        var cachedBytes  = await cache.GetAsync(cacheKey);
        if (cachedBytes is not null)
        {
            var cachedId = BitConverter.ToInt32(cachedBytes);
            identity.AddClaim(new Claim(DzdClaimTypes.DatabaseUserId, cachedId.ToString()));
            await next(context);
            return;
        }

        var email = GetClaimValue(context.User, ClaimTypes.Email, "email");
        var name  = GetClaimValue(context.User, ClaimTypes.Name, "name");
        var (firstName, lastName) = ParseName(name);

        try
        {
            var userId = await userService.SyncEntraUserAsync(objectId, email, firstName, lastName, context.RequestAborted);

            await cache.SetAsync(cacheKey, BitConverter.GetBytes(userId),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheDuration });

            identity.AddClaim(new Claim(DzdClaimTypes.DatabaseUserId, userId.ToString()));
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            return;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User sync failed for {Path} (objectId={ObjectId})",
                context.Request.Path, objectId);
            await WriteProblemAsync(context, StatusCodes.Status503ServiceUnavailable,
                "User Sync Failed", "User account synchronisation failed. Please try again.");
            return;
        }

        await next(context);
    }

    private static string? GetClaimValue(ClaimsPrincipal user, params string[] claimTypes)
    {
        foreach (var type in claimTypes)
        {
            var value = user.FindFirst(type)?.Value;
            if (value is not null) return value;
        }
        return null;
    }

    private const string CacheKeyPrefix = "entra-uid:";

    private static string CacheKey(string objectId) => $"{CacheKeyPrefix}{objectId}";

    private static async Task WriteProblemAsync(HttpContext context, int status, string title, string detail)
    {
        context.Response.StatusCode    = status;
        context.Response.ContentType   = "application/problem+json";
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new ProblemDetails { Status = status, Title = title, Detail = detail }));
    }

    private static (string? FirstName, string? LastName) ParseName(string? rawName)
    {
        if (string.IsNullOrWhiteSpace(rawName)) return (null, null);

        var normalized = WhitespaceRegex().Replace(rawName.Trim(), " ");
        var delim      = normalized.IndexOf(" - ", StringComparison.Ordinal);
        var segment    = (delim >= 0 ? normalized[..delim] : normalized).Trim();
        var tokens     = segment.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return tokens.Length switch
        {
            0 => (null, null),
            1 => (tokens[0], null),
            _ => (tokens[0], string.Join(" ", tokens[1..]))
        };
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}
