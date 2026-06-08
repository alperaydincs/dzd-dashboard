using DZDDashboard.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDependencies(builder.Configuration, builder.Environment);

var app = builder.Build();

app.ConfigureApiPipeline();

// Warn in production if no known proxy IPs are configured: the ForwardedHeaders middleware
// currently trusts all proxies (KnownNetworks/KnownProxies cleared for cloud LB compatibility).
// In production, restrict ForwardedHeadersOptions.KnownProxies to your load-balancer CIDRs
// to prevent X-Forwarded-For spoofing and incorrect rate-limiter IP partitioning.
if (!app.Environment.IsDevelopment())
{
    var knownProxies = app.Configuration.GetSection("Api:KnownProxies").Get<string[]>();
    if (knownProxies is null || knownProxies.Length == 0)
        app.Logger.LogWarning(
            "Api:KnownProxies is not configured. The app trusts all X-Forwarded-For headers. " +
            "Add your load-balancer IP(s) to Api:KnownProxies in appsettings to prevent header spoofing.");
}

await app.RunAsync();
