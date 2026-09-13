using Npgsql;

namespace CreateURL.Repositories.Database;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

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
        await command.ExecuteNonQueryAsync();
    }
}