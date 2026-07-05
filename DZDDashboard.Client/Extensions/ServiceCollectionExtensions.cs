using DZDDashboard.Client.Services;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using System.Net.Http;
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
        services.Configure<DownstreamApiOptions>(
            configuration.GetSection(DownstreamApiOptions.SectionName));

        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.Configure<Microsoft.AspNetCore.Builder.RequestLocalizationOptions>(options =>
        {
            options.SetDefaultCulture(DZDDashboard.Client.Localization.AppLocalizer.DefaultCulture);
            options.AddSupportedCultures(DZDDashboard.Client.Localization.AppLocalizer.SupportedCultures);
            options.AddSupportedUICultures(DZDDashboard.Client.Localization.AppLocalizer.SupportedCultures);
        });

        services.AddMudServices();
        services.AddAntiforgery();
        services.AddHttpContextAccessor();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required.");

        services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = connectionString;
            options.SchemaName       = "dbo";
            options.TableName        = "TokenCache";
        });

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
        services.AddScoped<DZDDashboard.Client.Localization.AppLocalizer>();
        services.AddScoped<DZDDashboard.Client.Localization.DomainLocalizer>();
        services.AddScoped<UserService>();
        services.AddScoped<IUserClientService>(sp => sp.GetRequiredService<UserService>());
        services.AddScoped<OrganizationService>();
        services.AddScoped<IOrganizationClientService>(sp => sp.GetRequiredService<OrganizationService>());
        services.AddScoped<PaymentService>();
        services.AddScoped<IPaymentClientService>(sp => sp.GetRequiredService<PaymentService>());
        services.AddScoped<OnboardingClientService>();
        services.AddScoped<IOnboardingClientService>(sp => sp.GetRequiredService<OnboardingClientService>());
        services.AddScoped<OffboardingClientService>();
        services.AddScoped<IOffboardingClientService>(sp => sp.GetRequiredService<OffboardingClientService>());
        services.AddScoped<MyOnboardingClientService>();
        services.AddScoped<IMyOnboardingClientService>(sp => sp.GetRequiredService<MyOnboardingClientService>());
        services.AddScoped<ChecklistTemplateClientService>();
        services.AddScoped<IChecklistTemplateClientService>(sp => sp.GetRequiredService<ChecklistTemplateClientService>());
        services.AddScoped<TrainingClientService>();
        services.AddScoped<ITrainingClientService>(sp => sp.GetRequiredService<TrainingClientService>());
        services.AddScoped<NotificationCenterService>();
        services.AddScoped<INotificationCenterService>(sp => sp.GetRequiredService<NotificationCenterService>());
        services.AddScoped<IUserAvatarState, UserAvatarState>();

        return services;
    }
}
