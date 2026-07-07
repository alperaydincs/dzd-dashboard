using Community.Microsoft.Extensions.Caching.PostgreSql;
using DZDDashboard.Api.Abstractions;
using Microsoft.AspNetCore.HttpOverrides;
using DZDDashboard.Api.Filters;
using DZDDashboard.Api.Options;
using DZDDashboard.Api.Validators;
using DZDDashboard.Common.Constants;
using DZDDashboard.Data;
using DZDDashboard.Data.Abstractions;
using DZDDashboard.Services;
using DZDDashboard.Services.Mapping;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Web;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Threading.RateLimiting;

namespace DZDDashboard.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddControllers(options => options.Filters.Add<ApiExceptionFilter>());
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CompanyDtoValidator>();
        services.AddOpenApi();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });

        var rl = configuration.GetSection("Api:RateLimit").Get<ApiRateLimitOptions>() ?? new ApiRateLimitOptions();
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddPolicy("api", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ResolveClientIp(context),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit          = rl.GeneralPermitLimit,
                        Window               = TimeSpan.FromMinutes(rl.GeneralWindowMinutes),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit           = 0
                    }));

            options.AddPolicy("upload", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ResolveClientIp(context),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rl.UploadPermitLimit,
                        Window      = TimeSpan.FromMinutes(rl.UploadWindowMinutes)
                    }));
        });

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required.");

        if (environment.IsDevelopment())
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            services.AddDistributedPostgreSqlCache(options =>
            {
                options.ConnectionString     = connectionString;
                options.SchemaName           = "public";
                options.TableName            = "ApiTokenCache";
                options.CreateInfrastructure = true;
            });
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services
            .AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
            {
                configuration.Bind("AzureAd", options);
                options.TokenValidationParameters.RoleClaimType = DzdClaimTypes.RoleClaimType;

                var clientId = configuration["AzureAd:ClientId"];
                if (!string.IsNullOrWhiteSpace(clientId))
                {
                    options.TokenValidationParameters.ValidAudiences =
                        [clientId, $"api://{clientId}"];
                }
            }, options => configuration.Bind("AzureAd", options));

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = options.DefaultPolicy;

            options.AddPolicy(Roles.AdminOrHrPolicy, policy =>
                policy.RequireRole(Roles.Admin, Roles.Hr));

            options.AddPolicy(Roles.SensitiveDataPolicy, policy =>
                policy.RequireRole(Roles.Admin, Roles.Hr));
        });

        services.Configure<ApiOptions>(configuration.GetSection("Api"));
        services.Configure<MiddlewareOptions>(configuration.GetSection("Middleware"));
        services.Configure<OnboardingOptions>(configuration.GetSection(OnboardingOptions.SectionName));
        services.AddUdemyIntegration(configuration);

        services.AddMemoryCache();
        services.AddApplicationServices();
        services.AddHttpContextAccessor();
        services.AddScoped<IAuditProvider, HttpContextAuditProvider>();
        services.AddScoped<ICurrentUserAccessor, HttpContextCurrentUserAccessor>();
        TypeAdapterConfig.GlobalSettings.Scan(typeof(OrganizationMappingProfile).Assembly);
        services.AddMapster();

        var allowedOrigins = configuration.GetSection("Api:AllowedCorsOrigins").Get<string[]>() ?? [];

        if (!environment.IsDevelopment() && allowedOrigins.Length == 0)
            throw new InvalidOperationException(
                "Api:AllowedCorsOrigins must contain at least one origin for non-development environments. " +
                "Add the Blazor client URL to appsettings.json or an environment variable override.");

        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()));

        services.AddHealthChecks()
            .AddNpgSql(connectionString, name: "sql", failureStatus: HealthStatus.Degraded);

        return services;
    }

    private static string ResolveClientIp(HttpContext context)
        => context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IReportsToCalculator, ReportsToCalculator>();
        services.AddScoped<UserService>();
        services.AddScoped<IUserReadService>(sp => sp.GetRequiredService<UserService>());
        services.AddScoped<IUserWriteService>(sp => sp.GetRequiredService<UserService>());
        services.AddScoped<IUserSyncService>(sp => sp.GetRequiredService<UserService>());
        services.AddScoped<ICompanyOrgService, CompanyOrgService>();
        services.AddScoped<IReferenceDataService, ReferenceDataService>();
        services.AddScoped<IOrganizationPositionService, OrganizationPositionService>();
        services.AddScoped<ICareerPathService, CareerPathService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IFileStorageService, DbFileStorageService>();
        services.AddScoped<IUserDocumentService, UserDocumentService>();
        services.AddScoped<LifecycleEngine>();
        services.AddScoped<IOnboardingService, OnboardingService>();
        services.AddScoped<IOffboardingService, OffboardingService>();
        services.AddScoped<IProcessTemplateService, ProcessTemplateService>();
        services.AddScoped<IChecklistTemplateService, ChecklistTemplateService>();
        services.AddScoped<IDocumentTemplateService, DocumentTemplateService>();
        services.AddScoped<ITrainingProgressService, TrainingProgressService>();
        return services;
    }

    private static IServiceCollection AddUdemyIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DZDDashboard.Api.Udemy.UdemyOptions>(
            configuration.GetSection(DZDDashboard.Api.Udemy.UdemyOptions.SectionName));

        var udemy = configuration.GetSection(DZDDashboard.Api.Udemy.UdemyOptions.SectionName)
            .Get<DZDDashboard.Api.Udemy.UdemyOptions>() ?? new DZDDashboard.Api.Udemy.UdemyOptions();

        services.AddHttpClient<DZDDashboard.Api.Udemy.IUdemyApiClient, DZDDashboard.Api.Udemy.UdemyApiClient>(client =>
        {
            if (!string.IsNullOrWhiteSpace(udemy.BaseUrl))
                client.BaseAddress = new Uri(udemy.BaseUrl);
            client.Timeout = TimeSpan.FromMinutes(2);
        });

        if (udemy.IsConfigured)
            services.AddHostedService<DZDDashboard.Api.Udemy.UdemySyncBackgroundService>();

        return services;
    }
}
