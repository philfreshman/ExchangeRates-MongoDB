using ExchangeRates.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ExchangeRates.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// Get a new access token using Basic authorization
    /// </summary>
    /// <returns></returns>
    public IActionResult GetToken()
    {
        var authorizationHeader = Request.Headers.Authorization.ToString();
        var splitAuthorizationHeader = authorizationHeader.Split(' ');
        if (splitAuthorizationHeader.Length < 2)
            return BadRequest();

        var authorizationHeaderDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(splitAuthorizationHeader[1]));
        var credentials = authorizationHeaderDecoded.Split(':');
        if (credentials.Length < 2)
            return BadRequest();

        var token = _tokenService.GenerateToken(credentials[0], credentials[1]);
        return token == null ? NotFound() : Ok(token);
    }
}
