using Dapper;
using OwOAuth.Core.Dependencies.Data;
using Entity = OwOAuth.Core.Entities;
using Model = OwOAuth.Infrastructure.Date.Models;

namespace OwOAuth.Infrastructure.Date;

internal sealed class SqliteUserRepository
    : BaseRepository, IUserRepository
{
    private const string TableName = "Users";

    public SqliteUserRepository(
        InfrastructureConfiguration configuration)
        : base(configuration)
    {
    }

    public async Task<IEnumerable<Entity.User>> GetUsersAsync(
        CancellationToken cancellationToken = default)
    {
        const string sql = $"SELECT * FROM {TableName}";

        using var connection = GetConnection();
        var models = await connection.QueryAsync<Model.User>(sql);

        var entities = models
            .Select(ModelToEntity)
            .ToList();

        return entities;
    }

    public async Task<Entity.User?> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        const string sql = $@"
            SELECT
                *
            FROM {TableName}
            WHERE UserId = @UserId";

        var param = new
        {
            UserId = userId.ToString(),
        };

        using var connection = GetConnection();
        var model = await connection.QueryFirstOrDefaultAsync<Model.User>(sql, param);
        if (model is null)
            return null;

        var entity = ModelToEntity(model);
        return entity;
    }

    public async Task<Entity.User?> GetUserByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        var sql = $@"
            SELECT
                *
            FROM {TableName}
            WHERE Username COLLATE NOCASE = @Username COLLATE NOCASE";

        var param = new
        {
            Username = username,
        };

        using var connection = GetConnection();
        var model = await connection.QueryFirstOrDefaultAsync<Model.User>(sql, param);
        if (model is null)
            return null;

        var entity = ModelToEntity(model);
        return entity;
    }

    public async Task<Guid> CreateUserAsync(
        Entity.User user,
        CancellationToken cancellationToken = default)
    {
        user.UserId = Guid.NewGuid();

        var model = EntityToModel(user);

        const string sql = $@"
            INSERT INTO {TableName}
            (
                UserId,
                Username,
                EmailAddress,
                Password
            )
            VALUES
            (
                @UserId,
                @Username,
                @EmailAddress,
                @Password
            )";

        var param = new
        {
            UserId = model.UserId.ToString(),
            Username = model.Username,
            Password = model.Password,
        };

        using var connection = GetConnection();
        var affectedRows = await connection.ExecuteAsync(sql, param);

        return user.UserId;
    }

    public async Task<bool> UpdateUserAsync(
        Entity.User user,
        CancellationToken cancellationToken = default)
    {
        var model = EntityToModel(user);

        const string sql = $@"
            UPDATE {TableName}
            SET
                Username = @Username,
                EmailAddress = @EmailAddress,
                Password = @Password
            WHERE
                UserId = @UserId";

        var param = new
        {
            UserId = model.UserId,
            Username = model.Username,
            EmailAddress = model.EmailAddress,
            Password = model.Password,
        };

        using var connection = GetConnection();
        var affectedRows = await connection.ExecuteAsync(sql, param);

        return affectedRows > 0;
    }

    public async Task<bool> DeleteUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        const string sql = $"DELETE FROM {TableName} WHERE UserId = @UserId";

        var param = new
        {
            UserId = userId.ToString(),
        };

        using var connection = GetConnection();
        var affectedRows = await connection.ExecuteAsync(sql, param);

        return affectedRows > 0;
    }

    private Entity.User ModelToEntity(
        Model.User model)
    {
        var entity = new Entity.User
        {
            UserId = Guid.Parse(model.UserId),
            Username = model.Username,
            EmailAddress = model.EmailAddress,
            Password = new Entity.Password
            {
                Salt = model.Password.Take(16).ToArray(),
                Hash = model.Password.Skip(16).ToArray(),
            },
        };
        return entity;
    }

    private Model.User EntityToModel(
        Entity.User entity)
    {
        var model = new Model.User
        {
            UserId = entity.UserId.ToString(),
            Username = entity.Username,
            EmailAddress = entity.EmailAddress,
            Password = entity.Password.Salt.Concat(entity.Password.Hash).ToArray(),
        };
        return model;
    }
}
