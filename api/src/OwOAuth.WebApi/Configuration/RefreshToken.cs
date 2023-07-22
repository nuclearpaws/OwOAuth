namespace OwOAuth.WebApi.Configuration;

public sealed class RefreshToken
{
    public TimeSpan LifeSpan { get; set; } = TimeSpan.FromDays(5);
}
