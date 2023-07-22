using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using OwOAuth.Core.Dependencies;

namespace OwOAuth.WebApi.Services;

public sealed class JwtProvider
    : IAccessTokenProvider
{
    private readonly byte[] _signingKey;

    public JwtProvider(
        byte[] signingKey,
        IDateTimeProvider dateTimeProvider)
    {
        _signingKey = signingKey;
    }

    public string Create(
        Guid userId,
        string username,
        DateTimeOffset validFrom,
        DateTimeOffset validTill)
    {
        var signingKey = new SymmetricSecurityKey(_signingKey);
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("username", username), // TODO: Extract this to some enum or some shiz...
        };

        var jwtToken = new JwtSecurityToken(
            "OwOAuth",
            "OwOAuth",
            claims,
            validFrom.UtcDateTime,
            validTill.UtcDateTime,
            signingCredentials);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtToken);
        return token;
    }
}
