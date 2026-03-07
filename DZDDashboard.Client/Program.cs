using DZDDashboard.Client.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddClientDependencies(builder.Configuration);

var app = builder.Build();

app.ConfigureClientPipeline();

await app.RunAsync();
