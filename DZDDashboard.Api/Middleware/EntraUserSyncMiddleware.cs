using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace DZDDashboard.Api.Middleware;

public class EntraUserSyncMiddleware
{
    private static readonly TimeSpan UserIdCacheDuration = TimeSpan.FromMinutes(15);

    private readonly RequestDelegate _next;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<EntraUserSyncMiddleware> _logger;

    public EntraUserSyncMiddleware(RequestDelegate next, IMemoryCache memoryCache, ILogger<EntraUserSyncMiddleware> logger)
    {
        _next = next;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var objectId = context.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                          ?? context.User.FindFirst("oid")?.Value
                          ?? context.User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(objectId))
            {
                await _next(context);
                return;
            }

            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null && identity.HasClaim(c => c.Type == "database_user_id"))
            {
                await _next(context);
                return;
            }

            var cacheKey = $"entra-user-id:{objectId}";
            if (_memoryCache.TryGetValue(cacheKey, out int cachedUserId))
            {
                identity?.AddClaim(new Claim("database_user_id", cachedUserId.ToString()));
                await _next(context);
                return;
            }

            var email = context.User.FindFirst(ClaimTypes.Email)?.Value
                       ?? context.User.FindFirst("email")?.Value;

            var name = context.User.FindFirst(ClaimTypes.Name)?.Value
                      ?? context.User.FindFirst("name")?.Value;

            try
            {
                var user = await db.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.EntraObjectId == objectId);

                if (user == null)
                {
                    var (firstName, lastName) = ParseName(name);

                    var newUser = new User
                    {
                        EntraObjectId = objectId,
                        Email = email,
                        NormalizedEmail = string.IsNullOrWhiteSpace(email) ? null : email.ToUpperInvariant(),
                        FirstName = firstName,
                        LastName = lastName,
                        IsActive = true
                    };

                    db.Users.Add(newUser);
                    await db.SaveChangesAsync();
                    user = newUser;
                }

                _memoryCache.Set(cacheKey, user.Id, UserIdCacheDuration);

                identity?.AddClaim(new Claim("database_user_id", user.Id.ToString()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sync Entra user {ObjectId} to database", objectId);
            }
        }

        await _next(context);
    }

    private static (string? FirstName, string? LastName) ParseName(string? rawName)
    {
        if (string.IsNullOrWhiteSpace(rawName))
            return (null, null);

        var normalized = Regex.Replace(rawName.Trim(), "\\s+", " ");
        var delimiterIndex = normalized.IndexOf(" - ", StringComparison.Ordinal);
        var personSegment = (delimiterIndex >= 0 ? normalized[..delimiterIndex] : normalized).Trim();
        var tokens = personSegment.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return tokens.Length switch
        {
            0 => (null, null),
            1 => (tokens[0], null),
            _ => (tokens[0], tokens[1])
        };
    }
}
