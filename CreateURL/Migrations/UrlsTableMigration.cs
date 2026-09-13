using BuildingBlocks.Data;
using BuildingBlocks.Migrations;
using Npgsql;

namespace CreateURL.Migrations;

public class UrlsTableMigration(IDbConnectionFactory connectionFactory) : IDatabaseMigration
{
    public async Task ApplyAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """
            CREATE TABLE IF NOT EXISTS urls (
                id UUID PRIMARY KEY,
                long_url TEXT NOT NULL,
                code TEXT NOT NULL UNIQUE,
                custom_alias TEXT,
                created_at TIMESTAMPTZ NOT NULL,
                expiration_date TIMESTAMPTZ
            );
            """;

        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
