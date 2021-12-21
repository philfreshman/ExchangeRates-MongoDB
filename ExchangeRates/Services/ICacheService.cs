using ExchangeRates.Models;

namespace ExchangeRates.Services;
public interface ICacheService
{
    Task<ExchangeRateResult> GetCachedResult(ExchangeRateRequest request);
    Task UpdateCachedResult(ExchangeRateResult result);
    Task BulkUpdateCachedResults(IEnumerable<ExchangeRateResult> results);
}
