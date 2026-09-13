namespace Api.Controllers;

public record CreateShortURLRequest(string LongURL, string? CustomAlias, DateTime? ExpirationDate);