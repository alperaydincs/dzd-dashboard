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
    private readonly ILogger<EntraUserSyncMiddleware> _logger;
    private readonly IMemoryCache _memoryCache;

    public EntraUserSyncMiddleware(RequestDelegate next, ILogger<EntraUserSyncMiddleware> logger, IMemoryCache memoryCache)
    {
        _next = next;
        _logger = logger;
        _memoryCache = memoryCache;
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
                if (identity != null)
                {
                    identity.AddClaim(new Claim("database_user_id", cachedUserId.ToString()));
                }

                await _next(context);
                return;
            }
            
            var email = context.User.FindFirst(ClaimTypes.Email)?.Value
                       ?? context.User.FindFirst("preferred_username")?.Value
                       ?? context.User.FindFirst("email")?.Value
                       ?? context.User.FindFirst(ClaimTypes.Upn)?.Value
                       ?? context.User.FindFirst("upn")?.Value
                       ?? context.User.FindFirst("unique_name")?.Value;
            
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

                if (identity != null)
                {
                    identity.AddClaim(new Claim("database_user_id", user.Id.ToString()));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while syncing Entra user");
            }
        }

        await _next(context);
    }

    private static (string? FirstName, string? LastName) ParseName(string? rawName)
    {
        if (string.IsNullOrWhiteSpace(rawName))
        {
            return (null, null);
        }

        var normalized = Regex.Replace(rawName.Trim(), "\\s+", " ");
        var delimiterIndex = normalized.IndexOf(" - ", StringComparison.Ordinal);
        var personSegment = (delimiterIndex >= 0 ? normalized[..delimiterIndex] : normalized).Trim();
        var tokens = personSegment
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (tokens.Length == 0)
        {
            return (null, null);
        }

        if (tokens.Length == 1)
        {
            return (tokens[0], null);
        }

        return (tokens[0], tokens[1]);
    }
}
