using OwOAuth.Core.Dependencies.Data;
using OwOAuth.Core.Dependencies.Security;

namespace OwOAuth.Core.UseCases.Users;

public sealed class UpdatePassword
    : IRequestHandler<
        UpdatePassword.Request,
        ResponseWrapper<UpdatePassword.Response>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICryptographicRandomProvider _cryptographicRandomProvider;
    private readonly IPasswordHasher _passwordHasher;

    public UpdatePassword(
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
        var user = await _userRepository
            .GetUserByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return new EntityNotFoundError(
                nameof(E.User),
                (nameof(E.User.UserId), request.UserId));

        var newSalt = _cryptographicRandomProvider.GetBytes(16);
        var newHash = _passwordHasher.HashPassword(newSalt, request.Password.NewPassword);

        user.Password.Salt = newSalt;
        user.Password.Hash = newHash;

        await _userRepository.UpdateUserAsync(user, cancellationToken);

        var response = new Response
        {
            UserId = user.UserId,
        };
        return response;
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
        public Guid UserId { get; set; }
        public PasswordDto Password { get; set; } = new();

        public sealed class PasswordDto
        {
            public string NewPassword { get; set; } = string.Empty;
        }
    }

    public sealed class Response
    {
        public Guid UserId { get; set; }
    }
}
