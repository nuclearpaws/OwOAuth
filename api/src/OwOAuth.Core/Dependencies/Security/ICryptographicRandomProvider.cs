namespace OwOAuth.Core.Dependencies.Security;

public interface ICryptographicRandomProvider
{
    byte[] GetBytes(int count);
}
