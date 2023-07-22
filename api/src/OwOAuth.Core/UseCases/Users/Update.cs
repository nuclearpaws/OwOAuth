using OwOAuth.Core.Dependencies.Data;

namespace OwOAuth.Core.UseCases.Users;

public sealed class Update
    : IRequestHandler<
        Update.Request,
        ResponseWrapper<Update.Response>>
{
    private readonly IUserRepository _userRepository;

    public Update(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetUserByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return new EntityNotFoundError(
                nameof(E.User),
                (nameof(E.User.UserId), request.UserId));

        user.Username = request.User.Username;

        await _userRepository.UpdateUserAsync(user, cancellationToken);

        var response = new Response
        {
            UserId = user.UserId,
            Username = user.Username,
        };
        return response;
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
        public Guid UserId { get; set; }
        public UserDto User { get; set; } = new();

        public sealed class UserDto
        {
            public string Username { get; set; } = string.Empty;
        }
    }

    public sealed class Response
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
