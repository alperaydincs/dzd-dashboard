using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DZDDashboard.Api.Services;

public class EntraUserClaimsTransformer : IClaimsTransformation
{
    private readonly IServiceProvider _serviceProvider;

    public EntraUserClaimsTransformer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (!principal.Identity?.IsAuthenticated ?? false)
            return principal;

        var objectId = principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                      ?? principal.FindFirst("oid")?.Value;
        var email = principal.FindFirst(ClaimTypes.Email)?.Value
                   ?? principal.FindFirst("preferred_username")?.Value
                   ?? principal.FindFirst("email")?.Value;
        var name = principal.FindFirst(ClaimTypes.Name)?.Value
                  ?? principal.FindFirst("name")?.Value;

        if (string.IsNullOrEmpty(objectId) || string.IsNullOrEmpty(email))
            return principal;

        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = await db.Users
            .FirstOrDefaultAsync(u => u.EntraObjectId == objectId);

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

        var identity = (ClaimsIdentity)principal.Identity!;
        identity.AddClaim(new Claim("database_user_id", user.Id.ToString()));

        return principal;
    }
}
