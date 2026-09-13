using Npgsql;

namespace CreateURL.Repositories.Database;

public class DatabaseRepository : IDatabaseRepository
{
    private readonly string _connectionString;

    public DatabaseRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<string?> GetByLongURL(string longURL)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = "SELECT code FROM urls WHERE long_url = @long_url";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@long_url", longURL);

        var result = await command.ExecuteScalarAsync();
        return result?.ToString();
    }

    public async Task<string> Add(URL url)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = """
            INSERT INTO urls (id, long_url, code, custom_alias, created_at, expiration_date)
            VALUES (@id, @long_url, @code, @custom_alias, @created_at, @expiration_date)
            """;

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", url.Id);
        command.Parameters.AddWithValue("@long_url", url.LongUrl);
        command.Parameters.AddWithValue("@code", url.Code);
        command.Parameters.AddWithValue("@custom_alias", (object?)url.CustomAlias ?? DBNull.Value);
        command.Parameters.AddWithValue("@created_at", url.CreatedAt);
        command.Parameters.AddWithValue("@expiration_date", (object?)url.ExpirationDate ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();

        return url.Code;
    }
}
