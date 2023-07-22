using OwOAuth.Core.Dependencies;
using OwOAuth.Core.Dependencies.Data;

namespace OwOAuth.Core.UseCases.Account;

public sealed class GetCurrent
    : IRequestHandler<
        GetCurrent.Request,
        ResponseWrapper<GetCurrent.Response>>
{
    private readonly IUserIdentityProvider _userIdentityProvider;
    private readonly IUserRepository _userRepository;

    public GetCurrent(
        IUserIdentityProvider userIdentityProvider,
        IUserRepository userRepository)
    {
        _userIdentityProvider = userIdentityProvider;
        _userRepository = userRepository;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        var userId = _userIdentityProvider.GetUserId();

        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user is null)
            return new EntityNotFoundError(
                nameof(E.User),
                (nameof(E.User.UserId), userId));

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
    }

    public sealed class Response
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
    }
}