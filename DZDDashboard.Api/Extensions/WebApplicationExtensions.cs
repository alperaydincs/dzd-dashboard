using DZDDashboard.Api.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DZDDashboard.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApiPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseDeveloperExceptionPage();
        }

        // UseForwardedHeaders must run before rate limiting and authentication so that
        // context.Connection.RemoteIpAddress is the real client IP (not the proxy IP).
        // KnownProxies / KnownNetworks are configured in ServiceCollectionExtensions.
        app.UseForwardedHeaders();
        app.UseHttpsRedirection();
        app.UseRateLimiter();
        app.UseCors();
        app.UseAuthentication();
        app.UseMiddleware<EntraUserSyncMiddleware>();
        app.UseAuthorization();
        app.MapControllers().RequireRateLimiting("api");

        // Health check endpoints (unauthenticated)
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy]   = StatusCodes.Status200OK,
                [HealthStatus.Degraded]  = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        }).AllowAnonymous();

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready")
        }).AllowAnonymous();

        return app;
    }
}
