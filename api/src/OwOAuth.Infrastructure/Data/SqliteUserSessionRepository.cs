using Dapper;
using OwOAuth.Core.Dependencies.Data;
using Entity = OwOAuth.Core.Entities;
using Model = OwOAuth.Infrastructure.Date.Models;

namespace OwOAuth.Infrastructure.Date;

internal sealed class SqliteUserSessionRepository
    : BaseRepository, IUserSessionRepository
{
    private const string TableName = "UserSessions";

    public SqliteUserSessionRepository(
        InfrastructureConfiguration configuration)
        : base(configuration)
    {
    }

    public async Task<Entity.UserSession?> GetUserSessionByIdAsync(
        Guid sessionId,
        CancellationToken cancellationToken = default)
    {
        const string sql = $@"
            SELECT
                *
            FROM {TableName}
            WHERE SessionId = @SessionId";

        var param = new
        {
            SessionId = sessionId.ToString(),
        };

        using var connection = GetConnection();
        var model = await connection.QuerySingleOrDefaultAsync<Model.UserSession?>(sql, param);
        if (model is null)
            return null;

        var entity = ModelToEntity(model);
        return entity;
    }

    public async Task<Entity.UserSession?> GetUserSessionByTokenValueAsync(
        byte[] tokenValue,
        CancellationToken cancellationToken = default)
    {
        const string sql = $@"
            SELECT
                *
            FROM {TableName}
            WHERE RefreshTokenValue = @RefreshTokenValue";

        var param = new
        {
            RefreshTokenValue = tokenValue,
        };

        using var connection = GetConnection();
        var model = await connection.QuerySingleOrDefaultAsync<Model.UserSession?>(sql, param);
        if (model is null)
            return null;

        var entity = ModelToEntity(model);
        return entity;
    }

    public async Task<IEnumerable<Entity.UserSession>> GetUserSessionsByUserIdAsync(
        Guid userId,
        bool includeRevoked,
        CancellationToken cancellationToken = default)
    {
        const string sql = $@"
            SELECT
                *
            FROM {TableName}
            WHERE UserId = @UserId
            AND (@IncludeRevoked = 'True' OR IsRevoked = 'False')";

        var param = new
        {
            UserId = userId.ToString(),
            IncludeRevoked = includeRevoked.ToString(),
        };

        using var connection = GetConnection();
        var models = await connection.QueryAsync<Model.UserSession>(sql, param);
        var entities = models
            .Select(ModelToEntity)
            .Where(e => e is not null)
            .Select(e => e!)
            .ToList();

        return entities;
    }

    public async Task<Guid> CreateUserSessionAsync(
        Entity.UserSession userSession,
        CancellationToken cancellationToken = default)
    {
        userSession.SessionId = Guid.NewGuid();

        var model = EntityToModel(userSession)!;

        const string sql = $@"
            INSERT INTO {TableName}
            (
                SessionId,
                UserId,
                RefreshTokenValue,
                Started,
                Expires,
                IsRevoked
            )
            VALUES
            (
                @SessionId,
                @UserId,
                @RefreshTokenValue,
                @Started,
                @Expires,
                @IsRevoked
            )";

        var param = new
        {
            SessionId = model.SessionId.ToString(),
            UserId = model.UserId.ToString(),
            RefreshTokenValue = model.RefreshTokenValue,
            Started = model.Started.ToString(),
            Expires = model.Expires.ToString(),
            IsRevoked = model.IsRevoked,
        };

        using var connection = GetConnection();
        var affectedRows = await connection.ExecuteAsync(sql, param);

        return userSession.SessionId;
    }

    public async Task<bool> RefreshUserSessionByIdAsync(
        Guid sessionId,
        byte[] newTokenValue,
        DateTimeOffset newExpires,
        CancellationToken cancellationToken = default)
    {
        const string sql = $@"
            UPDATE {TableName}
            SET
                RefreshTokenValue = @NewRefreshTokenValue,
                Expires = @NewExpires
            WHERE SessionId = @SessionId";

        var param = new
        {
            NewRefreshTokenValue = newTokenValue,
            NewExpires = newExpires,
            SessionId = sessionId.ToString(),
        };

        using var connection = GetConnection();
        var affectedRows = await connection.ExecuteAsync(sql, param);

        return affectedRows > 0;
    }

    public async Task<bool> RevokeUserSessionByIdAsync(
        Guid sessionId,
        CancellationToken cancellationToken = default)
    {
        const string sql = $@"
            UPDATE {TableName}
            SET
                IsRevoked = @IsRevoked
            WHERE
                SessionId = @SessionId";

        var param = new
        {
            IsRevoked = true.ToString(),
            SessionId = sessionId.ToString(),
        };

        using var connection = GetConnection();
        var affectedRows = await connection.ExecuteAsync(sql, param);

        return affectedRows > 0;
    }

    public async Task<int> RevokeUserSessionsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        const string sql = $@"
            UPDATE {TableName}
            SET
                IsRevoked = @IsRevoked
            WHERE
                UserId = @UserId";

        var param = new
        {
            IsRevoked = true.ToString(),
            UserId = userId.ToString(),
        };

        using var connection = GetConnection();
        var affectedRows = await connection.ExecuteAsync(sql, param);

        return affectedRows;
    }

    private static Entity.UserSession ModelToEntity(
        Model.UserSession model)
    {
        var entity = new Entity.UserSession
        {
            SessionId = Guid.Parse(model.SessionId),
            UserId = Guid.Parse(model.UserId),
            RefreshTokenValue = model.RefreshTokenValue,
            Started = DateTimeOffset.Parse(model.Started),
            Expires = DateTimeOffset.Parse(model.Expires),
            IsRevoked = bool.Parse(model.IsRevoked),
        };
        return entity;
    }

    private static Model.UserSession? EntityToModel(
        Entity.UserSession? entity)
    {
        if (entity is null)
            return null;

        var model = new Model.UserSession
        {
            SessionId = entity.SessionId.ToString(),
            UserId = entity.UserId.ToString(),
            RefreshTokenValue = entity.RefreshTokenValue,
            Started = entity.Started.ToString(),
            Expires = entity.Expires.ToString(),
            IsRevoked = entity.IsRevoked.ToString(),
        };
        return model;
    }
}
