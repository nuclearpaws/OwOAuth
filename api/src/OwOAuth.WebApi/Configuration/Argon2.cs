namespace OwOAuth.WebApi.Configuration;

public sealed class Argon2
{
    public byte[] Secret { get; set; } = Array.Empty<byte>();
}
