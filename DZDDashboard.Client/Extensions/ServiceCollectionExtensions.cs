using DZDDashboard.Client.Services;
using DZDDashboard.Common.Constants;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;

namespace DZDDashboard.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientDependencies(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        // Bind typed options before any service that depends on them
        services.Configure<DownstreamApiOptions>(
            configuration.GetSection(DownstreamApiOptions.SectionName));

        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddMudServices();
        services.RegisterIntlTelInput();
        services.AddAntiforgery();
        services.AddHttpContextAccessor();

        // Development: in-memory token cache (no DB table required).
        // Production: SQL Server-backed distributed cache (requires TokenCache table — create with
        //   dotnet sql-cache create "<conn>" dbo TokenCache).
        if (environment.IsDevelopment())
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required.");

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = connectionString;
                options.SchemaName       = "dbo";
                options.TableName        = "TokenCache";
            });
        }

        services
            .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(options =>
            {
                configuration.Bind("AzureAd", options);
                options.SaveTokens = true;
                options.TokenValidationParameters.RoleClaimType = DzdClaimTypes.RoleClaimType;

                var apiScope = configuration["DownstreamApi:Scopes"];
                if (!string.IsNullOrEmpty(apiScope))
                    options.Scope.Add(apiScope);
            })
            .EnableTokenAcquisitionToCallDownstreamApi(
                [configuration["DownstreamApi:Scopes"] ?? string.Empty])
            .AddDistributedTokenCaches();

        services.Configure<OpenIdConnectOptions>(
            OpenIdConnectDefaults.AuthenticationScheme,
            options => options.ResponseMode = "form_post");

        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan  = TimeSpan.FromDays(configuration.GetValue("Cookie:ExpireTimeSpanDays", 14));
            options.SlidingExpiration = configuration.GetValue("Cookie:SlidingExpiration", true);
        });

        services.AddCascadingAuthenticationState();
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(Roles.Admin,     policy => policy.RequireRole(Roles.Admin));
            options.AddPolicy(Roles.Hr,        policy => policy.RequireRole(Roles.Hr));
            options.AddPolicy(Roles.AdminOrHrPolicy, policy => policy.RequireRole(Roles.Admin, Roles.Hr));
        });
        services.AddControllersWithViews().AddMicrosoftIdentityUI();

        services.AddHttpClient("Api", client =>
        {
            client.BaseAddress = new Uri(
                configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("ApiBaseUrl configuration is missing."));
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddApplicationServices();

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<AuthTokenHandler>();
        services.AddScoped<UserService>();
        services.AddScoped<IUserClientService>(sp => sp.GetRequiredService<UserService>());
        services.AddScoped<OrganizationService>();
        services.AddScoped<IOrganizationClientService>(sp => sp.GetRequiredService<OrganizationService>());
        services.AddScoped<PaymentService>();
        services.AddScoped<IPaymentClientService>(sp => sp.GetRequiredService<PaymentService>());
        services.AddScoped<NotificationCenterService>();
        services.AddScoped<INotificationCenterService>(sp => sp.GetRequiredService<NotificationCenterService>());

        return services;
    }
}
