using DZDDashboard.Client.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;

namespace DZDDashboard.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddMudServices();
        services.AddAntiforgery();
        services.AddHttpContextAccessor();

        services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = configuration.GetConnectionString("DefaultConnection");
            options.SchemaName = "dbo";
            options.TableName = "TokenCache";
        });

        services
            .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(options =>
            {
                configuration.Bind("AzureAd", options);
                options.SaveTokens = true;
                options.TokenValidationParameters.RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

                var apiScope = configuration["DownstreamApi:Scopes"];
                if (!string.IsNullOrEmpty(apiScope))
                {
                    options.Scope.Add(apiScope);
                }
            })
            .EnableTokenAcquisitionToCallDownstreamApi(new[] { configuration["DownstreamApi:Scopes"] ?? string.Empty })
            .AddDistributedTokenCaches();

        services.Configure<OpenIdConnectOptions>(
            OpenIdConnectDefaults.AuthenticationScheme,
            options => options.ResponseMode = "form_post");

        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromDays(14);
            options.SlidingExpiration = true;
        });

        services.AddAuthorization();
        services.AddScoped<AuthTokenHandler>();
        services.AddControllersWithViews().AddMicrosoftIdentityUI();

        services.AddHttpClient("Api", client =>
        {
            client.BaseAddress = new Uri(configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("ApiBaseUrl missing"));
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddScoped<UserService>();
        services.AddScoped<OrganizationService>();
        services.AddScoped<NotificationCenterService>();

        return services;
    }
}
