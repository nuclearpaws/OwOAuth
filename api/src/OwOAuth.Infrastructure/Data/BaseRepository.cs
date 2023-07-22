using System.Data.Common;
using System.Data.SQLite;

namespace OwOAuth.Infrastructure.Date;

public abstract class BaseRepository
{
    private readonly string _connectionString;

    public BaseRepository(
        InfrastructureConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration.OwOAuthConnectionString))
            throw new ArgumentNullException(nameof(configuration));

        _connectionString = configuration.OwOAuthConnectionString;
    }

    protected DbConnection GetConnection()
    {
        var connection = new SQLiteConnection(_connectionString);
        return connection;
    }
}
