using Microsoft.Extensions.DependencyInjection;
using OwOAuth.Core.Middleware;

namespace OwOAuth.Core;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddOwOAuthCore(
        this IServiceCollection services,
        Action<CoreConfiguration>? configurationDelegate = null)
    {
        var configuration = new CoreConfiguration();
        configurationDelegate?.Invoke(configuration);
        services.AddSingleton(configuration);

        services.AddMediatR(config =>
        {
            var assembly = typeof(IServiceCollectionExtensions).Assembly;
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(PerformanceMonitor<,>));
        });

        return services;
    }
}
