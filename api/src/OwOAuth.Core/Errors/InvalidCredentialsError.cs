namespace OwOAuth.Core.Errors;

public sealed class InvalidCredentialsError
    : AbstractResponseError
{
    public InvalidCredentialsError()
        : base("Invalid Credentials Provided.")
    {
    }
}
