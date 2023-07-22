namespace OwOAuth.Core.Entities;

public sealed class Password
{
    public byte[] Salt { get; set; } = Array.Empty<byte>();
    public byte[] Hash { get; set; } = Array.Empty<byte>();
}
