namespace OwOAuth.Core.Dependencies;

public interface IUserIdentityProvider
{
    Guid GetUserId();
    string? GetUsername();
}
