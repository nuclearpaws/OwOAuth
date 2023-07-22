using OwOAuth.Core.Dependencies.Data;

namespace OwOAuth.Core.UseCases.Users;

public sealed class GetAll
    : IRequestHandler<
        GetAll.Request,
        ResponseWrapper<GetAll.Response>>
{
    private readonly IUserRepository _userRepository;

    public GetAll(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository
            .GetUsersAsync(cancellationToken);

        var response = new Response
        {
            Users = users
                .Select(u => new Response.UserDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                })
                .ToList(),
        };
        return response;
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
    }

    public sealed class Response
    {
        public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();

        public sealed class UserDto
        {
            public Guid UserId { get; set; }
            public string Username { get; set; } = string.Empty;
        }
    }
}
