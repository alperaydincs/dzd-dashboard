using DZDDashboard.Data;
using DZDDashboard.Services;
using DZDDashboard.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

namespace DZDDashboard.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddMemoryCache();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services
            .AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
            {
                configuration.Bind("AzureAd", options);
                options.TokenValidationParameters.RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

                var clientId = configuration["AzureAd:ClientId"];
                if (!string.IsNullOrWhiteSpace(clientId))
                {
                    options.TokenValidationParameters.ValidAudiences =
                    [
                        clientId,
                        $"api://{clientId}"
                    ];
                }
            }, options =>
            {
                configuration.Bind("AzureAd", options);
            });

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = options.DefaultPolicy;
        });

        services.AddScoped<UserService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<OrganizationPositionService>();
        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.WithOrigins("https://localhost:7111")
                      .AllowAnyHeader()
                      .AllowAnyMethod());
        });

        return services;
    }
}
