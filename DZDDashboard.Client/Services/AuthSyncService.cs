using System.Security.Claims;

namespace DZDDashboard.Client.Services;

public class AuthSyncService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthSyncService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task SyncUserAsync(ClaimsPrincipal user)
    {
        if (user?.Identity?.IsAuthenticated != true)
            return;

        var objectId = user.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier") 
                      ?? user.FindFirstValue("oid");
        var email = user.FindFirstValue(ClaimTypes.Email) 
                   ?? user.FindFirstValue("preferred_username") 
                   ?? user.FindFirstValue("email");
        var name = user.FindFirstValue(ClaimTypes.Name) 
                  ?? user.FindFirstValue("name");

        if (string.IsNullOrEmpty(objectId) || string.IsNullOrEmpty(email))
            return;

        try
        {
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.PostAsJsonAsync("auth/sync-user", new
            {
                objectId,
                email,
                name
            });

            response.EnsureSuccessStatusCode();
        }
        catch {  }
    }
}
