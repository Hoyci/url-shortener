namespace RedirectURL.Repositories.Redis;

public interface IRedisRepository
{
    Task<string?> GetByCode(string code);
    Task InsertURL(string code, string longURL);
}
