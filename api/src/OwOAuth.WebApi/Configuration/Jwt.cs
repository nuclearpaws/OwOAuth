namespace OwOAuth.WebApi.Configuration;

public sealed class Jwt
{
    public byte[] SigningKey { get; set; } = Array.Empty<byte>();
    public TimeSpan LifeSpan { get; set; } = TimeSpan.FromMinutes(5);
}
