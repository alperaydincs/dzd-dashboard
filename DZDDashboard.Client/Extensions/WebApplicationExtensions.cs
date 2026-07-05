using DZDDashboard.Client.Components;
using DZDDashboard.Client.Services;
using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureClientPipeline(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRequestLocalization();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        app.MapControllers();
        app.MapAvatarProxy();
        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .RequireAuthorization();

        return app;
    }

    // Same-origin avatar proxy: the browser requests /avatars/... with the app's auth cookie,
    // and this handler fetches the image from the downstream API using the user's on-behalf-of
    // bearer token. This is the single seam for avatar delivery — when avatars move to blob
    // storage, only these handlers change (e.g. redirect to the blob URL); callers keep using
    // the AvatarUrl helper. The ?v= cache-buster lets us cache each URL as immutable.
    private static void MapAvatarProxy(this WebApplication app)
    {
        // Current user's own avatar. Available to every authenticated user (self-service),
        // so the app-bar header can show it without requiring elevated roles.
        app.MapGet("/avatars/me", async (IUserClientService userService, HttpContext http) =>
                WriteAvatar(await userService.GetMyAvatarAsync(), http))
            .RequireAuthorization();

        // Another user's avatar. The downstream API allows any authenticated user to fetch it:
        // the org chart (visible to all authenticated users) already exposes names/emails/jobs
        // and renders these avatars, so the image is no more sensitive than data shown there.
        app.MapGet("/avatars/{userId:int}", async (int userId, IUserClientService userService, HttpContext http) =>
                WriteAvatar(await userService.GetUserAvatarAsync(userId), http))
            .RequireAuthorization();
    }

    private static IResult WriteAvatar(UserAvatarDto? avatar, HttpContext http)
    {
        if (string.IsNullOrEmpty(avatar?.ContentBase64))
            return Results.NotFound();

        var bytes = Convert.FromBase64String(avatar.ContentBase64);
        // The URL carries a ?v={avatar-modified-ticks} cache-buster, so a given URL always
        // maps to one immutable image — safe to cache aggressively. When the avatar changes,
        // the version (and thus the URL) changes, forcing the browser to fetch the new image.
        http.Response.Headers.CacheControl = "private, max-age=31536000, immutable";
        return Results.File(bytes, avatar.ContentType ?? "image/png");
    }
}
