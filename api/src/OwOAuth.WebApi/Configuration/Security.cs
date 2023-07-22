namespace OwOAuth.WebApi.Configuration;

public sealed class Security
{
    public RefreshToken RefreshToken { get; set; } = new();
    public Jwt Jwt { get; set; } = new();
    public Argon2 Argon2 { get; set; } = new();
}
