using OwOAuth.Core.Dependencies;
using OwOAuth.Core.Dependencies.Data;
using OwOAuth.Core.Dependencies.Security;

namespace OwOAuth.Core.UseCases.Account;

public sealed class UpdatePassword
    : IRequestHandler<
        UpdatePassword.Request,
        ResponseWrapper<UpdatePassword.Response>>
{
    private readonly IUserIdentityProvider _userIdentityProvider;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICryptographicRandomProvider _cryptographicRandomProvider;

    public UpdatePassword(
        IUserIdentityProvider userIdentityProvider,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ICryptographicRandomProvider cryptographicRandomProvider)
    {
        _userIdentityProvider = userIdentityProvider;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _cryptographicRandomProvider = cryptographicRandomProvider;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        var userId = _userIdentityProvider.GetUserId();

        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        if (user is null)
            return new EntityNotFoundError(
                nameof(E.User),
                (nameof(E.User.UserId), userId));

        var currentPasswordHashed = _passwordHasher
            .HashPassword(user.Password.Salt, request.CurrentPassword);

        if (!currentPasswordHashed.SequenceEqual(user.Password.Hash))
            return new InvalidCredentialsError(); // TODO: Probably a different error

        var newSalt = _cryptographicRandomProvider.GetBytes(16);
        var newHash = _passwordHasher.HashPassword(newSalt, request.NewPassword);

        user.Password.Salt = newSalt;
        user.Password.Hash = newHash;

        await _userRepository.UpdateUserAsync(user, cancellationToken);

        var response = new Response
        {
            UserId = userId,
        };
        return response;
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public sealed class Response
    {
        public Guid UserId { get; set; }
    }
}
