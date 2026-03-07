using DZDDashboard.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDependencies(builder.Configuration);

var app = builder.Build();

app.ConfigureApiPipeline();

await app.RunAsync();
