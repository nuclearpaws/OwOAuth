namespace OwOAuth.WebApi;

public sealed class WebApiConfiguration
{
    public byte[] JwtSigningKey { get; set; } = Array.Empty<byte>();
}
