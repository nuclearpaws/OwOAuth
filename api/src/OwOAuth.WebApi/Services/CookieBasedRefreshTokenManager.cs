using OwOAuth.Core.Dependencies;

namespace OwOAuth.WebApi.Services;

public sealed class CookieBasedRefreshTokenManager
    : IRefreshTokenManager
{
    private const string CookieName = "OwOAuthRefreshToken";

    private readonly HttpContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CookieBasedRefreshTokenManager(
        IHttpContextAccessor contextAccessor,
        IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;

        _context = contextAccessor.HttpContext
            ?? throw new InvalidOperationException($"Cannot access {nameof(HttpContext)}.");
    }

    public void Grant(string refreshToken)
    {
        var cookieOptions = GetCookieOptions();
        _context.Response.Cookies.Append(CookieName, refreshToken, cookieOptions);
    }

    public void Revoke()
    {
        var cookieOptions = GetCookieOptions();
        _context.Response.Cookies.Delete(CookieName, cookieOptions);
    }

    public bool TryGetValue(out string? value)
    {
        var cookieValue = _context
            .Request
            .Cookies
            .Where(c => c.Key == CookieName)
            .Select(c => c.Value)
            .FirstOrDefault();

        var cookieExists = cookieValue is not null;

        value = cookieExists
            ? cookieValue
            : string.Empty;

        return cookieExists;
    }

    private CookieOptions GetCookieOptions()
    {
        var now = _dateTimeProvider.GetNow();
        var expires = now.AddDays(7);

        var options = new CookieOptions
        {
            Path = "api/login/refresh",
            Expires = expires,
            Secure = true,
            HttpOnly = true,
            IsEssential = true,
        };
        return options;
    }
}
