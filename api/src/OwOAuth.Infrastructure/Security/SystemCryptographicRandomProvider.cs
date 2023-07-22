using System.Security.Cryptography;
using OwOAuth.Core.Dependencies.Security;

namespace OwOAuth.Infrastructure.Security;

internal sealed class SystemCryptographicRandomProvider
    : ICryptographicRandomProvider
{
    public byte[] GetBytes(int count)
    {
        var bytes = RandomNumberGenerator.GetBytes(count);
        return bytes;
    }
}
