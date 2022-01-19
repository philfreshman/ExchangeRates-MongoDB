using ExchangeRates.Models;
using ExchangeRates.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRates.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RatesController : ControllerBase
{
    private readonly IRatesService _ratesService;
    private readonly ICacheService _cacheService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<RatesController> _logger;

    public RatesController(IRatesService ratesService, ICacheService cacheService, ITokenService tokenService, ILogger<RatesController> logger)
    {
        _ratesService = ratesService;
        _cacheService = cacheService;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetExchangeRates(
        [FromBody] Dictionary<string, string> currencyCodes,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string apiKey)
    {
        if (!_tokenService.Tokens.Contains(apiKey))
            return Unauthorized();

        if (startDate > DateTime.Now || endDate > DateTime.Now)
            return NotFound();
        if (endDate == null)
            endDate = startDate;

        try
        {
            var cacheTasks = currencyCodes.Select(kvp =>
            {
                var request = new ExchangeRateRequest
                {
                    SourceCurrencyCode = kvp.Key,
                    TargetCurrencyCode = kvp.Value,
                    StartDate = startDate.Truncate(),
                    EndDate = endDate.Value.Truncate()
                };
                return _cacheService.GetCachedResult(request);
            }).ToList();
            await Task.WhenAll(cacheTasks);
            var cacheResults = cacheTasks.Select(t => t.Result).ToList();

            var remoteTasks = cacheResults.Where(r => r.Data == null).Select(r => _ratesService.GetExchangeRates(r.Request)).ToList();
            await Task.WhenAll(remoteTasks);
            var remoteResults = remoteTasks.Select(t => t.Result).ToList();
            await _cacheService.BulkUpdateCachedResults(remoteResults);

            var concatenatedResults = cacheResults.Where(r => r.Data != null).Concat(remoteResults);
            return Ok(concatenatedResults);
        }
        catch (HttpException ex)
        {
            return StatusCode((int)ex.StatusCode);
        }
    }
}