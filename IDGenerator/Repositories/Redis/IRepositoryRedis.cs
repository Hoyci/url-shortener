namespace IDGenerator.Repositories.Redis;

public interface IRepositoryRedis
{
    Task<long> GetCount();
}
