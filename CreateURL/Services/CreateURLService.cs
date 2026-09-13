using IDGenerator.Services.IDGenerator;
using CreateURL.Repositories.Database;

namespace CreateURL.Services;

public class CreateURLService(
        IDatabaseRepository repository,
        IIDGeneratorService _IDGeneratorService
    ) : ICreateURLService
{
    public async Task<string> Create(string longURL, string? customAlias, DateTime? expirationDate)
    {
        var existingURL = await repository.GetByLongURL(longURL);
        if (existingURL is not null)
            return existingURL;

        var customCode = await _IDGeneratorService.Generate();
        var url = new URL(longURL, customCode, customAlias, expirationDate);

        return await repository.Add(url);
    }

}
