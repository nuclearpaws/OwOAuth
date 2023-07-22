using OwOAuth.Core.Dependencies;
using OwOAuth.Core.Dependencies.Data;

namespace OwOAuth.Core.UseCases.Sessions;

public sealed class GetAllForUser
    : IRequestHandler<
        GetAllForUser.Request,
        ResponseWrapper<GetAllForUser.Response>>
{
    private readonly IUserIdentityProvider _userIdentityProvider;
    private readonly IUserSessionRepository _userSessionRepository;

    public GetAllForUser(
        IUserIdentityProvider userIdentityProvider,
        IUserSessionRepository userSessionRepository)
    {
        _userIdentityProvider = userIdentityProvider;
        _userSessionRepository = userSessionRepository;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        var userId = _userIdentityProvider.GetUserId();

        var userSession = await _userSessionRepository
            .GetUserSessionsByUserIdAsync(
                userId,
                request.IncludeRevoked,
                cancellationToken);

        var response = new Response
        {
            UserSessions = userSession
                .Select(us => new Response.UserSessionDto
                {
                    SessionId = us.SessionId,
                    Started = us.Started,
                    Expires = us.Expires,
                    IsRevoked = us.IsRevoked,
                })
                .ToList(),
        };
        return response;
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
        public bool IncludeRevoked { get; set; } = false;
    }

    public sealed class Response
    {
        public IEnumerable<UserSessionDto> UserSessions { get; set; } = new List<UserSessionDto>();

        public sealed class UserSessionDto
        {
            public Guid SessionId { get; set; }
            public DateTimeOffset Started { get; set; }
            public DateTimeOffset Expires { get; set; }
            public bool IsRevoked { get; set; }
        }
    }
}
