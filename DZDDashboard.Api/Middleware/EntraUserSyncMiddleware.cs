using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace DZDDashboard.Api.Middleware;

public class EntraUserSyncMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<EntraUserSyncMiddleware> _logger;

    public EntraUserSyncMiddleware(RequestDelegate next, ILogger<EntraUserSyncMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var claims = context.User.Claims.ToList();
        
            var roles = context.User.FindAll("roles").Select(c => c.Value).ToList();

            foreach (var role in roles)
            {
                _logger.LogInformation($"  Role: {role}");
            }
            
            var objectId = context.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                          ?? context.User.FindFirst("oid")?.Value
                          ?? context.User.FindFirst("sub")?.Value;
            
            _logger.LogInformation($"ObjectId: {objectId}");
            
            var email = context.User.FindFirst(ClaimTypes.Email)?.Value
                       ?? context.User.FindFirst("preferred_username")?.Value
                       ?? context.User.FindFirst("email")?.Value
                       ?? context.User.FindFirst(ClaimTypes.Upn)?.Value
                       ?? context.User.FindFirst("upn")?.Value
                       ?? context.User.FindFirst("unique_name")?.Value;
            
            var name = context.User.FindFirst(ClaimTypes.Name)?.Value
                      ?? context.User.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(objectId))
            {
                try
                {
                    var user = await db.Users
                        .FirstOrDefaultAsync(u => u.EntraObjectId == objectId);

                    if (user == null)
                    {


                        var nameParts = name?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                        var baseUsername = BuildBaseUsername(email, objectId);
                        var username = await EnsureUniqueUsernameAsync(db, baseUsername, objectId);
                        
                        user = new User
                        {
                            EntraObjectId = objectId,
                            Email = email,
                            NormalizedEmail = string.IsNullOrWhiteSpace(email) ? null : email.ToUpperInvariant(),
                            Username = username,
                            NormalizedUsername = username.ToUpperInvariant(),
                            FirstName = nameParts.Length > 0 ? nameParts[0] : null,
                            LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : null,
                            IsActive = true
                        };

                        db.Users.Add(user);
                        await db.SaveChangesAsync();
                    }

                    var identity = context.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        if (!identity.HasClaim("database_user_id", user.Id.ToString()))
                        {
                            identity.AddClaim(new Claim("database_user_id", user.Id.ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in user sync middleware: {ex.Message}");
                }
            }
        }

        await _next(context);
    }

    private static string BuildBaseUsername(string? emailOrLogin, string objectId)
    {
        var source = string.IsNullOrWhiteSpace(emailOrLogin) ? objectId : emailOrLogin;

        if (!string.IsNullOrWhiteSpace(emailOrLogin) && emailOrLogin.Contains('@'))
        {
            source = emailOrLogin.Split('@')[0];
        }

        var cleaned = Regex.Replace(source, "[^a-zA-Z0-9._-]", "_");
        return string.IsNullOrWhiteSpace(cleaned) ? $"user_{objectId[..Math.Min(8, objectId.Length)]}" : cleaned;
    }

    private static async Task<string> EnsureUniqueUsernameAsync(AppDbContext db, string baseUsername, string objectId)
    {
        var normalized = baseUsername.ToUpperInvariant();
        var exists = await db.Users.AnyAsync(u => u.NormalizedUsername == normalized);
        if (!exists)
        {
            return baseUsername;
        }

        var suffix = objectId.Length >= 6 ? objectId[^6..] : objectId;
        var candidate = $"{baseUsername}_{suffix}";
        var counter = 1;

        while (await db.Users.AnyAsync(u => u.NormalizedUsername == candidate.ToUpperInvariant()))
        {
            candidate = $"{baseUsername}_{suffix}_{counter}";
            counter++;
        }

        return candidate;
    }
}
