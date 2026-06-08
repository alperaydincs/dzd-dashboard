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
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Web;
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

        // ForwardedHeaders: resolve real client IP from X-Forwarded-For / X-Forwarded-Proto
        // before rate limiting and authentication run. UseForwardedHeaders() is called in the pipeline.
        // In production, restrict KnownProxies/KnownNetworks to your load-balancer's CIDR.
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            // Clear default loopback-only restriction so Azure / cloud LBs can forward headers.
            // Restrict to specific proxy IPs in production via options.KnownProxies.Add(IPAddress.Parse("...")).
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        // Rate limiting — IP-based fixed window, limits configured in appsettings Api:RateLimit.
        // After UseForwardedHeaders middleware runs, context.Connection.RemoteIpAddress is the
        // real client IP — no manual header parsing needed.
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
            // Distributed cache — safe under multi-instance deployments
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = connectionString;
                options.SchemaName       = "dbo";
                options.TableName        = "ApiTokenCache";
            });
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

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

            // AdminOrHrPolicy — Admin or HR can perform write operations on employee data.
            options.AddPolicy(Roles.AdminOrHrPolicy, policy =>
                policy.RequireRole(Roles.Admin, Roles.Hr));

            // SensitiveDataPolicy — governs access to employee PII (citizenship, personal contact, etc.).
            // To add a new role: append it here AND add the role to Azure Entra ID.
            options.AddPolicy(Roles.SensitiveDataPolicy, policy =>
                policy.RequireRole(Roles.Admin, Roles.Hr));
        });

        services.Configure<ApiOptions>(configuration.GetSection("Api"));
        services.Configure<MiddlewareOptions>(configuration.GetSection("Middleware"));

        services.AddApplicationServices();
        services.AddHttpContextAccessor();
        services.AddScoped<IAuditProvider, HttpContextAuditProvider>();
        services.AddScoped<ICurrentUserAccessor, HttpContextCurrentUserAccessor>();
        services.AddAutoMapper(typeof(OrganizationMappingProfile).Assembly);

        var allowedOrigins = configuration.GetSection("Api:AllowedCorsOrigins").Get<string[]>() ?? [];

        // Guard: an empty CORS origin list silently blocks all cross-origin requests in production.
        // Fail fast at startup so misconfiguration is caught before deploying.
        if (!environment.IsDevelopment() && allowedOrigins.Length == 0)
            throw new InvalidOperationException(
                "Api:AllowedCorsOrigins must contain at least one origin for non-development environments. " +
                "Add the Blazor client URL to appsettings.json or an environment variable override.");

        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()));

        // Health checks
        services.AddHealthChecks()
            .AddSqlServer(connectionString, name: "sql", failureStatus: HealthStatus.Degraded);

        return services;
    }

    /// <summary>
    /// Returns the client IP for rate-limiter partitioning.
    /// By the time this runs, <c>UseForwardedHeaders</c> middleware has already resolved
    /// <c>RemoteIpAddress</c> from the <c>X-Forwarded-For</c> chain, so no manual
    /// header parsing is needed (and safe against header-spoofing attacks).
    /// </summary>
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
        return services;
    }
}
