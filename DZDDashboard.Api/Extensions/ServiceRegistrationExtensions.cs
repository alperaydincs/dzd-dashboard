namespace DZDDashboard.Api.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, Action<IServiceCollection> configure)
    {
        configure(services);
        return services;
    }

    public static IServiceCollection AddScoped<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddScoped<TInterface, TImplementation>(services);
        return services;
    }

    public static IServiceCollection AddScoped<TService>(this IServiceCollection services)
        where TService : class
    {
        Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddScoped<TService>(services);
        return services;
    }
}
