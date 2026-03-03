using DZDDashboard.Api.Services;
using DZDDashboard.Api.Middleware;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
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

        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.RoleClaimType = "roles"; 
        
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents();
    }, options =>
    {
        builder.Configuration.Bind("AzureAd", options);
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
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

await SeedAsync(app.Services);

await app.RunAsync();

static async Task SeedAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!await db.Roles.AnyAsync())
    {
        db.Roles.AddRange(new Role { Name = "Admin" }, new Role { Name = "HR" });
        await db.SaveChangesAsync();
    }
}
