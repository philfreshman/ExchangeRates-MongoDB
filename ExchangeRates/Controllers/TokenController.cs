using System.Text;
using ExchangeRates;
using Microsoft.AspNetCore.Mvc;
using ExchangeRates.Services;

namespace myExchangeRates.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [HttpGet]
    public IActionResult GetToken()
    {
        var authHeader = Request.Headers.Authorization.ToString();
        if (_tokenService.IsRequestEmptyOrNull(authHeader))
            return Unauthorized();
        
        var authEncoded = authHeader.Split(' ')[1];
        var authDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(authEncoded));
        var (name,password) = authDecoded.Split(":");
        if(_tokenService.CheckCredentials(name, password))
            return Unauthorized();

        var token = _tokenService.GenerateNewToken();
        return Ok(token);
    }
}