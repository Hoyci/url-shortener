namespace Api.Controllers;

/// <summary>
/// Dados necessários para criar uma URL encurtada.
/// </summary>
/// <param name="LongURL">URL original que será encurtada.</param>
/// <param name="CustomAlias">Alias customizado opcional para o código da URL.</param>
/// <param name="ExpirationDate">Data de expiração opcional da URL encurtada.</param>
public record CreateShortURLRequest(string LongURL, string? CustomAlias, DateTime? ExpirationDate);
