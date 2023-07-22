namespace OwOAuth.Infrastructure.Date.Models;

public sealed class User
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public byte[] Password { get; set; } = Array.Empty<byte>();
}
