using System.Diagnostics;
using OwOAuth.Core;
using OwOAuth.Infrastructure;
using OwOAuth.WebApi;
using OwOAuth.WebApi.Configuration;
using Serilog;
using Serilog.Events;

try
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();

    var builder = CreateBuilder();
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal("Application terminated unexpectedly.");
    Debug.WriteLine(ex);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

WebApplicationBuilder CreateBuilder()
{
    var builder = WebApplication.CreateBuilder(args);

    // CONFIGURATION //
    var environmentName = builder.Environment.EnvironmentName;
    builder.Configuration.AddJsonFile("appsettings.json", false);
    builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", true);

    var appSettings = new AppSettings();
    builder.Configuration.Bind(appSettings);

    // LOGGING //
    builder
        .Logging
        .ClearProviders()
        .AddSerilog(new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override(nameof(Microsoft), LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger());

    // SERVICES //
    builder.Services
        .AddOwOAuthCore(config =>
        {
            config.RefreshTokenLifeSpan = appSettings.Security.RefreshToken.LifeSpan;
            config.AccessTokenLifeSpan = appSettings.Security.Jwt.LifeSpan;
        })
        .AddOwOAuthInfrastructure(config =>
        {
            config.OwOAuthConnectionString = appSettings.ConnectionStrings.OwOAuthSqlite;
            config.Argon2Secret = appSettings.Security.Argon2.Secret;
        })
        .AddOwOAuthWebApi(config =>
        {
            config.JwtSigningKey = appSettings.Security.Jwt.SigningKey;
        });

    return builder;
}
