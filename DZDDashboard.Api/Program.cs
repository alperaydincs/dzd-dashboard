using DZDDashboard.Api.Services;
using DZDDashboard.Api.Middleware;
using DZDDashboard.Data;
using DZDDashboard.Services;
using DZDDashboard.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using DZDDashboard.Common.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services
        .AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAd", options);
            options.TokenValidationParameters.RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        var clientId = builder.Configuration["AzureAd:ClientId"];
        if (!string.IsNullOrWhiteSpace(clientId))
        {
            options.TokenValidationParameters.ValidAudiences = new[]
            {
                clientId,
                $"api://{clientId}"
            };
        }
    }, options =>
    {
        builder.Configuration.Bind("AzureAd", options);
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<OrganizationPositionService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("https://localhost:7111")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseMiddleware<EntraUserSyncMiddleware>();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
