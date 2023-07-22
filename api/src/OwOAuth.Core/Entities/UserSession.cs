namespace OwOAuth.Core.Entities;

public sealed class UserSession
{
    public Guid SessionId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public byte[] RefreshTokenValue { get; set; } = Array.Empty<byte>();
    public DateTimeOffset Started { get; set; }
    public DateTimeOffset Expires { get; set; }
    public bool IsRevoked { get; set; } = false;
}
