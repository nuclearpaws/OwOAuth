namespace OwOAuth.WebApi.Configuration;

public sealed class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = new();
    public Security Security { get; set; } = new();
}
