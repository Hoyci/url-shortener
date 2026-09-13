namespace RedirectURL.Repositories.Database;

public interface IDatabaseRepository
{
    Task<string?> GetByCode(string code);
}