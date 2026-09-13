using Npgsql;

namespace BuildingBlocks.Data;

public interface IDbConnectionFactory
{
    Task<NpgsqlConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
