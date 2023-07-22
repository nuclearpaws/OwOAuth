namespace OwOAuth.Core.Dependencies;

public interface IAccessTokenProvider
{
    string Create(
        Guid userId,
        string username,
        DateTimeOffset validFrom,
        DateTimeOffset validTill);
}
