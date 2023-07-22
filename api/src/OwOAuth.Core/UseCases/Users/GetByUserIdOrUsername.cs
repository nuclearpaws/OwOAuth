using OwOAuth.Core.Dependencies.Data;

namespace OwOAuth.Core.UseCases.Users;

public sealed class GetByUserIdOrUsername
    : IRequestHandler<
        GetByUserIdOrUsername.Request,
        ResponseWrapper<GetByUserIdOrUsername.Response>>
{
    private readonly IUserRepository _userRepository;

    public GetByUserIdOrUsername(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        var user = default(E.User);
        if (Guid.TryParse(request.UserIdOrUsername, out var userId))
        {
            user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (user is null)
                return new EntityNotFoundError(
                    nameof(E.User),
                    (nameof(E.User.UserId), userId));
        }
        else
        {
            user = await _userRepository.GetUserByUsernameAsync(request.UserIdOrUsername, cancellationToken);
            if (user is null)
                return new EntityNotFoundError(
                    nameof(E.User),
                    (nameof(E.User.Username), request.UserIdOrUsername));
        }

        var response = new Response
        {
            UserId = user.UserId,
            Username = user.Username,
            EmailAddress = user.EmailAddress,
        };
        return response;
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
        public string UserIdOrUsername { get; set; } = string.Empty;
    }

    public sealed class Response
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
    }
}
