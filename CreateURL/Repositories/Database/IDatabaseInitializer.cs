namespace CreateURL.Repositories.Database;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}