using OwOAuth.Core.Dependencies.Data;
using OwOAuth.Core.Dependencies.Security;

namespace OwOAuth.Core.UseCases.Users;

public sealed class Create
    : IRequestHandler<
        Create.Request,
        ResponseWrapper<Create.Response>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICryptographicRandomProvider _cryptographicRandomProvider;
    private readonly IPasswordHasher _passwordHasher;

    public Create(
        IUserRepository userRepository,
        ICryptographicRandomProvider cryptographicRandomProvider,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _cryptographicRandomProvider = cryptographicRandomProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        var salt = _cryptographicRandomProvider.GetBytes(16);
        var hash = _passwordHasher.HashPassword(salt, request.Password);

        var password = new E.Password
        {
            Salt = salt,
            Hash = hash,
        };

        var user = new E.User
        {
            Username = request.Username,
            EmailAddress = request.EmailAddress,
            Password = password,
        };

        var userId = await _userRepository
            .CreateUserAsync(user, cancellationToken);

        var response = new Response
        {
            UserId = userId,
            Username = user.Username,
            EmailAddress = user.EmailAddress,
        };
        return response;
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public sealed class Response
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
    }
}
