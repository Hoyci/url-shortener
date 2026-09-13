using Npgsql;
using BuildingBlocks.Data;
namespace RedirectURL.Repositories.Database;

public class DatabaseRepository(IDbConnectionFactory connectionFactory) : IDatabaseRepository
{
    public async Task<string?> GetByCode(string code)
    {
        await using var connection = await connectionFactory.CreateConnectionAsync();

        const string sql = "SELECT long_url FROM urls WHERE code = @code";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@code", code);

        var result = await command.ExecuteScalarAsync();
        return result?.ToString();
    }
}