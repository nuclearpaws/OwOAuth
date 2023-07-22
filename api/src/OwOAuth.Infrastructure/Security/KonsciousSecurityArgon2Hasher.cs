using OwOAuth.Core.Dependencies.Security;
using Konscious.Security.Cryptography;
using System.Text;

namespace OwOAuth.Infrastructure.Security;

internal sealed class KonsciousSecurityArgon2Hasher
    : IPasswordHasher
{
    private readonly byte[] _secret;

    public KonsciousSecurityArgon2Hasher(
        InfrastructureConfiguration configuration)
    {
        if (configuration.Argon2Secret is null)
            throw new ArgumentNullException(nameof(configuration));

        if (configuration.Argon2Secret.Length != 32)
            throw new ArgumentNullException("Argument must be 32 bytes long.", nameof(configuration));

        _secret = configuration.Argon2Secret;
    }

    public byte[] HashPassword(
        byte[] salt,
        string password)
    {
        if (salt is null)
            throw new ArgumentNullException(nameof(salt));

        if (salt.Length != 16)
            throw new ArgumentException("Argument must be 16 bytes long.", nameof(salt));

        var bytes = Encoding.UTF8.GetBytes(password);
        var argon2 = new Argon2id(bytes)
        {
            Salt = salt,
            DegreeOfParallelism = 8,
            KnownSecret = _secret,
            MemorySize = 65536,
            Iterations = 4,
        };

        var hash = argon2.GetBytes(32);
        return hash;
    }
}
