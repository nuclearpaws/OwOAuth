using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwOAuth.Core.UseCases.Account;

namespace OwOAuth.WebApi.Controllers;

[Authorize]
public sealed class AccountController
    : BaseMediatorController
{
    [HttpGet]
    public async Task<ActionResult<GetCurrent.Response>> GetAsync()
    {
        var request = new GetCurrent.Request();
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response => Ok(response));
        return result;
    }

    [HttpPost("UpdatePassword")]
    public async Task<ActionResult<UpdatePassword.Response>> PostUpdatePasswordAsync(
        [FromBody] UpdatePassword.Request request)
    {
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response => Ok(response));
        return result;
    }
}
