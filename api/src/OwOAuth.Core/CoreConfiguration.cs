namespace OwOAuth.Core;

public sealed class CoreConfiguration
{
    public TimeSpan AccessTokenLifeSpan { get; set; }
    public TimeSpan RefreshTokenLifeSpan { get; set; }
}
