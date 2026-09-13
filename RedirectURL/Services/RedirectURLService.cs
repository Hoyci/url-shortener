using RedirectURL.Repositories.Database;
using RedirectURL.Repositories.Redis;

namespace RedirectURL.Services;

public class RedirectURLService(
        IRedisRepository _redisRepository,
        IDatabaseRepository databaseRepository
    ) : IRedirectURLService
{
    public async Task<string> Redirect(string code)
    {
        var longURL = await _redisRepository.GetByCode(code);
        if (longURL is null)
        {
            Console.WriteLine("URL not found in Redis, checking database...");
            longURL = await databaseRepository.GetByCode(code) ?? throw new Exception("URL not found");
            await _redisRepository.InsertURL(code, longURL);
        }

        return longURL;
    }
}