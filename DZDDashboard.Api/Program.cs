using DZDDashboard.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDependencies(builder.Configuration, builder.Environment);

var app = builder.Build();

app.ConfigureApiPipeline();

if (!app.Environment.IsDevelopment())
{
    var knownProxies = app.Configuration.GetSection("Api:KnownProxies").Get<string[]>();
    if (knownProxies is null || knownProxies.Length == 0)
        app.Logger.LogWarning(
            "Api:KnownProxies is not configured. The app trusts all X-Forwarded-For headers. " +
            "Add your load-balancer IP(s) to Api:KnownProxies in appsettings to prevent header spoofing.");
}

await app.RunAsync();
