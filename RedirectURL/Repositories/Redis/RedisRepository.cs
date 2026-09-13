using StackExchange.Redis;

namespace RedirectURL.Repositories.Redis;

public class RedisRepository : IRedisRepository
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<string?> GetByCode(string code)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var result = await db.StringGetAsync(code);
        return result.HasValue ? result.ToString() : null;
    }

    public async Task InsertURL(string code, string longURL)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.StringSetAsync(code, longURL);
    }
}
