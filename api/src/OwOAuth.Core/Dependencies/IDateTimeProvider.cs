namespace OwOAuth.Core.Dependencies;

public interface IDateTimeProvider
{
    DateTimeOffset GetNow();
}
