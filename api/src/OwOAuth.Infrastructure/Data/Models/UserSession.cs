namespace OwOAuth.Infrastructure.Date.Models;

public sealed class UserSession
{
    public string SessionId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public byte[] RefreshTokenValue { get; set; } = Array.Empty<byte>();
    public string Started { get; set; } = string.Empty;
    public string Expires { get; set; } = string.Empty;
    public string IsRevoked { get; set; } = string.Empty;
}
