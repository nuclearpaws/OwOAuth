using OwOAuth.Core.Dependencies;
using OwOAuth.Core.Dependencies.Data;

namespace OwOAuth.Core.UseCases.Login;

public sealed class Logout
    : IRequestHandler<
        Logout.Request,
        ResponseWrapper<Logout.Response>>
{
    private readonly IRefreshTokenManager _refreshTokenManager;
    private readonly IUserSessionRepository _userSessionRepository;

    public Logout(
        IRefreshTokenManager refreshTokenManager,
        IUserSessionRepository userSessionRepository)
    {
        _refreshTokenManager = refreshTokenManager;
        _userSessionRepository = userSessionRepository;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        if (_refreshTokenManager.TryGetValue(out var refreshTokenValue))
            await RevokeUserSessionAsync(refreshTokenValue, cancellationToken);

        _refreshTokenManager.Revoke();

        var response = new Response();
        return response;
    }

    private async Task RevokeUserSessionAsync(
        string? refreshTokenString,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenString))
            return;

        var refreshTokenBytes = Convert.FromBase64String(refreshTokenString);

        var userSession = await _userSessionRepository
            .GetUserSessionByTokenValueAsync(
                refreshTokenBytes, cancellationToken);

        if (userSession is null)
            return;

        await _userSessionRepository
            .RevokeUserSessionByIdAsync(userSession.SessionId);
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
    }

    public sealed class Response
    {
    }
}
