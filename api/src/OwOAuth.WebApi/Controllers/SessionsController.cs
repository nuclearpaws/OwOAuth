using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwOAuth.Core.UseCases.Sessions;

namespace OwOAuth.WebApi.Controllers;

[Authorize]
public sealed class SessionsController
    : BaseMediatorController
{
    [HttpGet]
    public async Task<ActionResult<GetAllForUser.Response>> GetAllForUserAsync(
        [FromQuery] GetAllForUser.Request request)
    {
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response => Ok(response));
        return result;
    }

    [HttpPost("Revoke/{sessionId}")]
    public async Task<ActionResult> PostRevokeAsync(
        [FromRoute] Guid sessionId)
    {
        var request = new Revoke.Request
        {
            SessionId = sessionId,
        };
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response => Ok(response));
        return result;
    }
}
