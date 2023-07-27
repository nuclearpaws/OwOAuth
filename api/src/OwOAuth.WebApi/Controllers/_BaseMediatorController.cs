using MediatR;
using Microsoft.AspNetCore.Mvc;
using OwOAuth.Core.Errors;
using ResponseWrapper;

namespace OwOAuth.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseMediatorController
    : ControllerBase
{
    protected IMediator Mediator => HttpContext
        .RequestServices
        .GetRequiredService<IMediator>();

    protected ActionResult UnwrapResponseWrapper<TResponse>(
        ResponseWrapper<TResponse> wrappedResponse,
        Func<TResponse, ActionResult> responseHandler)
    {
        var result = wrappedResponse
            .Unwrap<ActionResult>(
                responseHandler,
                baseError => baseError switch
                {
                    EntityNotFoundError error => NotFound(error),
                    InvalidCredentialsError error => Unauthorized(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError),
                });
        return result;
    }
}