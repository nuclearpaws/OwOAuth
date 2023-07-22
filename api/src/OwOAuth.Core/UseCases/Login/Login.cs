using OwOAuth.Core.Dependencies;
using OwOAuth.Core.Dependencies.Data;
using OwOAuth.Core.Dependencies.Security;

namespace OwOAuth.Core.UseCases.Login;

public sealed class Login
    : IRequestHandler<
        Login.Request,
        ResponseWrapper<Login.Response>>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICryptographicRandomProvider _cryptographicRandomProvider;
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IRefreshTokenManager _refreshTokenManager;
    private readonly IAccessTokenProvider _accessTokenProvider;

    public Login(
        IDateTimeProvider dateTimeProvider,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ICryptographicRandomProvider cryptographicRandomProvider,
        IUserSessionRepository userSessionRepository,
        IRefreshTokenManager refreshTokenManager,
        IAccessTokenProvider accessTokenProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _cryptographicRandomProvider = cryptographicRandomProvider;
        _userSessionRepository = userSessionRepository;
        _refreshTokenManager = refreshTokenManager;
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetUserByUsernameAsync(request.Username);

        if (user is null)
            return new InvalidCredentialsError();

        var passwordHash = _passwordHasher
            .HashPassword(user.Password.Salt, request.Password);

        if (!passwordHash.SequenceEqual(user.Password.Hash))
            return new InvalidCredentialsError();

        var now = _dateTimeProvider.GetNow();

        var refreshToken = await GetRefreshTokenAsync(
            now, user, cancellationToken);
        _refreshTokenManager.Grant(refreshToken);

        var accessToken = GetAccessToken(now, user);

        var response = new Response
        {
            AccessToken = accessToken,
        };
        return response;
    }

    private async Task<string> GetRefreshTokenAsync(
        DateTimeOffset now,
        E.User user,
        CancellationToken cancellationToken)
    {
        var refreshTokenValue = _cryptographicRandomProvider
            .GetBytes(32);

        var started = now;
        var expires = now.AddDays(5);

        var userSession = new E.UserSession
        {
            UserId = user.UserId,
            RefreshTokenValue = refreshTokenValue,
            Started = started,
            Expires = expires,
            IsRevoked = false,
        };
        await _userSessionRepository.CreateUserSessionAsync(
            userSession, cancellationToken);

        return Convert.ToBase64String(refreshTokenValue);
    }

    private string GetAccessToken(
        DateTimeOffset now,
        E.User user)
    {
        var validFrom = now;
        var validTill = validFrom.AddMinutes(5);

        var userId = user.UserId;
        var username = user.Username;

        var accessToken = _accessTokenProvider
            .Create(
                userId,
                username,
                validFrom,
                validTill);

        return accessToken;
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public sealed class Response
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
