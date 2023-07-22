namespace OwOAuth.Core.Entities;

public sealed class User
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public Password Password { get; set; } = new();
}
