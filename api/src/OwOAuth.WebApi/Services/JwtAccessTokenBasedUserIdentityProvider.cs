using System.IdentityModel.Tokens.Jwt;
using OwOAuth.Core.Dependencies;

namespace OwOAuth.WebApi.Services;

internal sealed class JwtAccessTokenBasedUserIdentityProvider
    : IUserIdentityProvider
{
    private readonly HttpContext _context;

    public JwtAccessTokenBasedUserIdentityProvider(
        IHttpContextAccessor httpContextAccessor)
    {
        _context = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException($"Cannot access {nameof(HttpContext)}.");
    }

    public Guid GetUserId()
    {
        var token = GetToken();
        if (!Guid.TryParse(token.Subject, out var userId))
            throw new InvalidOperationException("Token has malformed 'UserId'.");

        return userId;
    }

    public string? GetUsername()
    {
        var token = GetToken();
        var username = token
            .Claims
            .Where(c => c.Type == "username") // TODO: Extract this to some enum or some shiz...
            .Select(c => c.Value)
            .FirstOrDefault();
        return username;
    }

    private JwtSecurityToken GetToken()
    {
        if (!_context.Request.Headers.TryGetValue("Authorization", out var bearerTokens))
            throw new InvalidOperationException("User aunauthorized!");

        var jwt = bearerTokens.Single().Replace("bearer", string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim();

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(jwt);
        return jwtToken;
    }
}
