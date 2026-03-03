using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DZDDashboard.Api.Middleware;

public class EntraUserSyncMiddleware
{
    private readonly RequestDelegate _next;

    public EntraUserSyncMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var objectId = context.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                          ?? context.User.FindFirst("oid")?.Value;
            
            var email = context.User.FindFirst(ClaimTypes.Email)?.Value
                       ?? context.User.FindFirst("preferred_username")?.Value
                       ?? context.User.FindFirst("email")?.Value;
            
            var name = context.User.FindFirst(ClaimTypes.Name)?.Value
                      ?? context.User.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(objectId) && !string.IsNullOrEmpty(email))
            {
                try
                {
                    var user = await db.Users.FirstOrDefaultAsync(u => u.EntraObjectId == objectId);

                    if (user == null)
                    {
                        var nameParts = name?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                        
                        user = new User
                        {
                            EntraObjectId = objectId,
                            Email = email,
                            NormalizedEmail = email.ToUpper(),
                            Username = email.Split('@')[0],
                            NormalizedUsername = email.Split('@')[0].ToUpper(),
                            FirstName = nameParts.Length > 0 ? nameParts[0] : null,
                            LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : null,
                            IsActive = true
                        };

                        db.Users.Add(user);
                        await db.SaveChangesAsync();
                    }

                    var identity = context.User.Identity as ClaimsIdentity;
                    if (identity != null && !identity.HasClaim("database_user_id", user.Id.ToString()))
                    {
                        identity.AddClaim(new Claim("database_user_id", user.Id.ToString()));
                    }
                }
                catch {  }
            }
        }

        await _next(context);
    }
}
