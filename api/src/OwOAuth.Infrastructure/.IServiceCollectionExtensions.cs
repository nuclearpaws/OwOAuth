using Microsoft.Extensions.DependencyInjection;
using OwOAuth.Core.Dependencies.Data;
using OwOAuth.Core.Dependencies.Security;
using OwOAuth.Infrastructure.Date;
using OwOAuth.Infrastructure.Security;

namespace OwOAuth.Infrastructure;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddOwOAuthInfrastructure(
        this IServiceCollection services,
        Action<InfrastructureConfiguration>? configurationDelegate = null)
    {
        var configuration = new InfrastructureConfiguration();
        configurationDelegate?.Invoke(configuration);
        services.AddSingleton(configuration);

        services.AddTransient<IUserRepository, SqliteUserRepository>();
        services.AddTransient<IUserSessionRepository, SqliteUserSessionRepository>();
        services.AddSingleton<IPasswordHasher, KonsciousSecurityArgon2Hasher>();
        services.AddSingleton<ICryptographicRandomProvider, SystemCryptographicRandomProvider>();

        return services;
    }
}
