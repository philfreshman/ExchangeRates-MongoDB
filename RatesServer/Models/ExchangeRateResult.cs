namespace ExchangeRates.Models;
public class ExchangeRateResult
{
    public ExchangeRateRequest Request { get; set; }

    public IList<ExchangeRate>? Data { get; set; }

    public ExchangeRateResult() { }
    public ExchangeRateResult(ExchangeRateRequest request)
    {
        Request = request;
    }
}