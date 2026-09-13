using CreateURL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class URLController(ICreateURLService createURLService) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateShortURL([FromBody] CreateShortURLRequest request)
    {
        var code = await createURLService.Create(request.LongURL, request.CustomAlias, request.ExpirationDate);
        return Ok(code);
    }
}