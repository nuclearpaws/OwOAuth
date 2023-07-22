namespace OwOAuth.Core.Dependencies.Data;

public interface IUserSessionRepository
{
    Task<E.UserSession?> GetUserSessionByIdAsync(
        Guid sessionId,
        CancellationToken cancellationToken = default);

    Task<E.UserSession?> GetUserSessionByTokenValueAsync(
        byte[] tokenValue,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<E.UserSession>> GetUserSessionsByUserIdAsync(
        Guid userId,
        bool includeRevoked,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateUserSessionAsync(
        E.UserSession userSession,
        CancellationToken cancellationToken = default);

    Task<bool> RefreshUserSessionByIdAsync(
        Guid sessionId,
        byte[] newTokenValue,
        DateTimeOffset newExpires,
        CancellationToken cancellationToken = default);

    Task<bool> RevokeUserSessionByIdAsync(
        Guid sessionId,
        CancellationToken cancellationToken = default);

    Task<int> RevokeUserSessionsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
