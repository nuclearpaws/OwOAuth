namespace OwOAuth.Core.Dependencies.Security;

public interface IPasswordHasher
{
    public byte[] HashPassword(
        byte[] salt,
        string password);
}
