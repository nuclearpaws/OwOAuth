using OwOAuth.Core.Dependencies;
using OwOAuth.Core.Dependencies.Data;
using OwOAuth.Core.Dependencies.Security;

namespace OwOAuth.Core.UseCases.Login;

public sealed class Refresh
    : IRequestHandler<
        Refresh.Request,
        ResponseWrapper<Refresh.Response>>
{
    private readonly IRefreshTokenManager _refreshTokenManager;
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserRepository _userRepository;
    private readonly ICryptographicRandomProvider _cryptographicRandomProvider;
    private readonly IAccessTokenProvider _accessTokenProvider;

    private readonly TimeSpan _accessTokenLifespan;
    private readonly TimeSpan _refreshTokenLifespan;

    public Refresh(
        IRefreshTokenManager refreshTokenManager,
        IUserSessionRepository userSessionRepository,
        IDateTimeProvider dateTimeProvider,
        IUserRepository userRepository,
        ICryptographicRandomProvider cryptographicRandomProvider,
        IAccessTokenProvider accessTokenProvider,
        CoreConfiguration configuration)
    {
        _refreshTokenManager = refreshTokenManager;
        _userSessionRepository = userSessionRepository;
        _dateTimeProvider = dateTimeProvider;
        _userRepository = userRepository;
        _accessTokenProvider = accessTokenProvider;
        _cryptographicRandomProvider = cryptographicRandomProvider;

        _accessTokenLifespan = configuration.AccessTokenLifeSpan;
        _refreshTokenLifespan = configuration.RefreshTokenLifeSpan;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        if (!_refreshTokenManager.TryGetValue(out var refreshTokenString))
        {
            _refreshTokenManager.Revoke();
            return new InvalidCredentialsError();
        }

        if (string.IsNullOrWhiteSpace(refreshTokenString))
        {
            _refreshTokenManager.Revoke();
            return new InvalidCredentialsError();
        }

        var refreshTokenBytes = Convert.FromBase64String(refreshTokenString);
        var userSession = await _userSessionRepository
            .GetUserSessionByTokenValueAsync(
                refreshTokenBytes, cancellationToken);

        if (userSession is null)
        {
            _refreshTokenManager.Revoke();
            return new InvalidCredentialsError();
        }

        var now = _dateTimeProvider.GetNow();

        if (userSession.Started > now || userSession.Expires < now)
        {
            _refreshTokenManager.Revoke();
            return new InvalidCredentialsError();
        }

        if (userSession.IsRevoked)
        {
            _refreshTokenManager.Revoke();
            return new InvalidCredentialsError();
        }

        var user = await _userRepository.GetUserByIdAsync(
            userSession.UserId, cancellationToken);

        if (user is null)
        {
            _refreshTokenManager.Revoke();
            return new EntityNotFoundError(
                nameof(E.User),
                (nameof(E.User.UserId), userSession.UserId));
        }

        var newRefreshToken = await GetNewRefreshTokenAsync(
            now, userSession, cancellationToken);
        _refreshTokenManager.Grant(newRefreshToken);
        var newAccessToken = GetNewAccessToken(now, user);

        var response = new Response
        {
            NewAccessToken = newAccessToken,
        };
        return response;
    }

    private async Task<string> GetNewRefreshTokenAsync(
        DateTimeOffset now,
        E.UserSession userSession,
        CancellationToken cancellationToken)
    {
        var newRefreshTokenValue = _cryptographicRandomProvider
            .GetBytes(32);

        var newExpires = now.Add(_refreshTokenLifespan);

        await _userSessionRepository.RefreshUserSessionByIdAsync(
            userSession.SessionId,
            newRefreshTokenValue,
            newExpires,
            cancellationToken);

        return Convert.ToBase64String(newRefreshTokenValue);
    }

    private string GetNewAccessToken(
        DateTimeOffset now,
        E.User user)
    {
        var validFrom = now;
        var validTill = validFrom.Add(_accessTokenLifespan);

        var userId = user.UserId;
        var username = user.Username;

        var accessToken = _accessTokenProvider.Create(
            userId, username, validFrom, validTill);

        return accessToken;
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
    }

    public sealed class Response
    {
        public string NewAccessToken { get; set; } = string.Empty;
    }
}
