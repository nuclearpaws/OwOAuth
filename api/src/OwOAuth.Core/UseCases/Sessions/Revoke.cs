using OwOAuth.Core.Dependencies;
using OwOAuth.Core.Dependencies.Data;

namespace OwOAuth.Core.UseCases.Sessions;

public sealed class Revoke
    : IRequestHandler<
        Revoke.Request,
        ResponseWrapper<Revoke.Response>>
{
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IUserIdentityProvider _userIdentityProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    public Revoke(
        IUserSessionRepository userSessionRepository,
        IUserIdentityProvider userIdentityProvider,
        IDateTimeProvider dateTimeProvider)
    {
        _userSessionRepository = userSessionRepository;
        _userIdentityProvider = userIdentityProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        var session = await _userSessionRepository
            .GetUserSessionByIdAsync(
                request.SessionId,
                cancellationToken);

        if (session is null)
            return new InvalidCredentialsError();

        var userId = _userIdentityProvider.GetUserId();
        if (session.UserId != userId)
            return new InvalidCredentialsError();

        if (session.IsRevoked)
            return new InvalidCredentialsError();

        var now = _dateTimeProvider.GetNow();
        if (session.Expires <= now)
            return new InvalidCredentialsError();

        var isRevokeSuccessful = await _userSessionRepository
            .RevokeUserSessionByIdAsync(session.SessionId);

        var response = new Response();
        return response;
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
        public Guid SessionId { get; set; }
    }

    public sealed class Response
    {
    }
}