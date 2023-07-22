namespace OwOAuth.Core.Dependencies.Data;

public interface IUserRepository
{
    public Task<IEnumerable<E.User>> GetUsersAsync(
        CancellationToken cancellationToken = default);

    public Task<E.User?> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    public Task<E.User?> GetUserByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    public Task<Guid> CreateUserAsync(
        E.User user,
        CancellationToken cancellationToken = default);

    public Task<bool> UpdateUserAsync(
        E.User user,
        CancellationToken cancellationToken = default);

    public Task<bool> DeleteUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

}
