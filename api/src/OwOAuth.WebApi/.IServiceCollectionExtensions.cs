using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OwOAuth.Core.Dependencies;
using OwOAuth.WebApi.Services;

namespace OwOAuth.WebApi;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddOwOAuthWebApi(
        this IServiceCollection services,
        Action<WebApiConfiguration>? configurationDelegate = null)
    {
        var configuration = new WebApiConfiguration();
        configurationDelegate?.Invoke(configuration);
        services.AddSingleton(configuration);

        // WEB API SERVICES:
        services.AddTransient<IRefreshTokenManager, CookieBasedRefreshTokenManager>();
        services.AddTransient<IUserIdentityProvider, JwtAccessTokenBasedUserIdentityProvider>();

        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<IAccessTokenProvider>(sc =>
        {
            var dateTimeProvider = sc.GetRequiredService<IDateTimeProvider>();
            var jwtProvider = new JwtProvider(configuration.JwtSigningKey, dateTimeProvider);
            return jwtProvider;
        });

        // ASP NET SHIZ:
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validate Signing:
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(configuration.JwtSigningKey),
                    ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
                    // Validate Expiration:
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    // Validate Claims:
                    ValidateIssuer = true,
                    ValidIssuers = new[] { nameof(OwOAuth) },
                    ValidateAudience = true,
                    ValidAudiences = new[] { nameof(OwOAuth) },
                };
            });

        services.AddAuthorization();

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(swaggerGenOptions =>
        {
            var openApiInfo = new OpenApiInfo
            {
                Title = "Your API",
                Version = "v1"
            };
            swaggerGenOptions.SwaggerDoc("v1", openApiInfo);

            // Fix schmea ids in OpenAPI spec
            swaggerGenOptions.CustomSchemaIds(type =>
            {
                var schema = type
                    .FullName!
                    .Replace('+', '.')
                    .Replace(nameof(OwOAuth), string.Empty)
                    .Replace(nameof(OwOAuth.Core), string.Empty)
                    .Replace(nameof(OwOAuth.Core.UseCases), string.Empty)
                    .Trim('.');

                return schema;
            });

            // Add JWT authentication support in Swagger UI
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                BearerFormat = "JWT",
                Scheme = "bearer",
                Description = "Specify the authorization token.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
            };
            swaggerGenOptions.AddSecurityDefinition("Bearer", securityScheme);
            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                    },
                    new string[] { }
                }
            });
        });

        services.AddHttpContextAccessor();

        return services;
    }
}
