using Microsoft.AspNetCore.Mvc;
using OwOAuth.Core.UseCases.Login;

namespace OwOAuth.WebApi.Controllers;

public sealed class LoginController
    : BaseMediatorController
{
    [HttpPost("Login")]
    public async Task<ActionResult<Login.Response>> PostLoginAsync(
        [FromBody] Login.Request request)
    {
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response => Ok(response));
        return result;
    }

    [HttpPost("Logout")]
    public async Task<ActionResult> PostLogoutAsync()
    {
        var request = new Logout.Request();
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            _ => NoContent());
        return result;
    }

    [HttpPost("Refresh")]
    public async Task<ActionResult<Refresh.Response>> PostRefreshAsync()
    {
        var request = new Refresh.Request();
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response => Ok(response));
        return result;
    }
}
