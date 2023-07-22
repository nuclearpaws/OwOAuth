namespace OwOAuth.Infrastructure;

public sealed class InfrastructureConfiguration
{
    public string OwOAuthConnectionString { get; set; } = string.Empty;
    public byte[] Argon2Secret { get; set; } = Array.Empty<byte>();
}
