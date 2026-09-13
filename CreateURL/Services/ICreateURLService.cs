namespace CreateURL.Services;

public interface ICreateURLService
{
    Task<string> Create(string longURL, string? customAlias, DateTime? expirationDate);
}