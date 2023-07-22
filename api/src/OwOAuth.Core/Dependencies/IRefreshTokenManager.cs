namespace OwOAuth.Core.Dependencies;

public interface IRefreshTokenManager
{
    void Grant(string refreshToken);
    void Revoke();
    bool TryGetValue(out string? value);
}
