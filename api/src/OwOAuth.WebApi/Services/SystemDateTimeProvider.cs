using OwOAuth.Core.Dependencies;

namespace OwOAuth.WebApi.Services;

public sealed class SystemDateTimeProvider
    : IDateTimeProvider
{
    public DateTimeOffset GetNow()
    {
        var now = DateTimeOffset.UtcNow;
        return now;
    }
}
