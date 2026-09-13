using CreateURL.Services;
using RedirectURL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Endpoints para encurtar e redirecionar URLs.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class URLController(
    ICreateURLService createURLService,
    IRedirectURLService redirectURLService
) : ControllerBase
{
    /// <summary>
    /// Cria uma URL encurtada.
    /// </summary>
    /// <param name="request">Dados da URL a ser encurtada.</param>
    /// <response code="200">Retorna o código da URL encurtada.</response>
    /// <response code="400">Requisição inválida.</response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateShortURL([FromBody] CreateShortURLRequest request)
    {
        try
        {
            var code = await createURLService.Create(request.LongURL, request.CustomAlias, request.ExpirationDate);
            return Ok(code);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Redireciona para a URL original a partir do código encurtado.
    /// </summary>
    /// <param name="code">Código da URL encurtada.</param>
    /// <response code="302">Redireciona para a URL original.</response>
    /// <response code="404">Código não encontrado.</response>
    [HttpGet("{code}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RedirectURL([FromRoute] string code)
    {
        try
        {
            var longURL = await redirectURLService.Redirect(code);
            return Redirect(longURL);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error redirecting URL: {ex.Message}");
            return NotFound();
        }
    }
}