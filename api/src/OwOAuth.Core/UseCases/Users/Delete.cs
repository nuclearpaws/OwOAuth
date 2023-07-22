using OwOAuth.Core.Dependencies.Data;

namespace OwOAuth.Core.UseCases.Users;

public sealed class Delete
    : IRequestHandler<
        Delete.Request,
        ResponseWrapper<Delete.Response>>
{
    private readonly IUserRepository _userRepository;

    public Delete(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResponseWrapper<Response>> Handle(
        Request request,
        CancellationToken cancellationToken)
    {
        await _userRepository
            .DeleteUserAsync(request.UserId, cancellationToken);

        var response = new Response();
        return response;
    }

    public sealed class Request
        : IRequest<ResponseWrapper<Response>>
    {
        public Guid UserId { get; set; }
    }

    public sealed class Response
    {
    }
}