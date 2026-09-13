namespace CreateURL.Repositories.Database;

public interface IDatabaseRepository
{
    Task<string?> GetByLongURL(string longURL);
    Task<string> Add(URL url);
}
