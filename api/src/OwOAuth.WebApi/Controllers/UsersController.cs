using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwOAuth.Core.UseCases.Users;

namespace OwOAuth.WebApi.Controllers;

[Authorize]
public sealed class UsersController
    : BaseMediatorController
{
    [HttpGet("")]
    public async Task<ActionResult<GetAll.Response>> GetAsync()
    {
        var request = new GetAll.Request();
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response => Ok(response));
        return result;
    }

    [HttpGet("{userIdOrUsername}")]
    public async Task<ActionResult<GetByUserIdOrUsername.Response>> GetByUserIdAsync(
        [FromRoute] string userIdOrUsername)
    {
        var request = new GetByUserIdOrUsername.Request
        {
            UserIdOrUsername = userIdOrUsername,
        };
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response => Ok(response));
        return result;
    }

    [HttpPost("")]
    public async Task<ActionResult<Create.Response>> PostAsync(
        [FromBody] Create.Request request)
    {
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response =>
            {
                var uri = $"/api/users/{response.UserId}";
                return Created(uri, response);
            });
        return result;
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<Update.Response>> PutByUserIdAsync(
        [FromRoute] Guid userId,
        [FromBody] Update.Request.UserDto user)
    {
        var request = new Update.Request
        {
            UserId = userId,
            User = user,
        };
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response => Ok(response));
        return result;
    }

    [HttpPut("{userId}/Password")]
    public async Task<ActionResult<UpdatePassword.Response>> PutByUserIdPasswordAsync(
        [FromRoute] Guid userId,
        [FromBody] UpdatePassword.Request.PasswordDto password)
    {
        var request = new UpdatePassword.Request
        {
            UserId = userId,
            Password = password,
        };
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response => Ok(response));
        return result;
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult<Delete.Response>> DeleteByUserIdAsync(
        [FromRoute] Guid userId)
    {
        var request = new Delete.Request
        {
            UserId = userId,
        };
        var wrappedResponse = await Mediator.Send(request);
        var result = UnwrapResponseWrapper(
            wrappedResponse,
            response => Ok(response));
        return result;
    }
}
