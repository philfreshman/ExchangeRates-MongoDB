using ExchangeRates.Models;

namespace ExchangeRates.Services;
public interface IRatesService
{
    Task<ExchangeRateResult> GetExchangeRates(ExchangeRateRequest request);
}