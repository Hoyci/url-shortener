using StackExchange.Redis;

namespace IDGenerator.Repositories.Redis;

public class Redis : IRepositoryRedis
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public Redis(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<long> GetCount()
    {
        var db = _connectionMultiplexer.GetDatabase();
        var count = await db.StringIncrementAsync("id_counter");
        return count;
    }
}
