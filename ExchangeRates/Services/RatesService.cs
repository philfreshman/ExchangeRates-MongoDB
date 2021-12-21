using ExchangeRates.Models;
using Newtonsoft.Json.Linq;
using System.Web;

namespace ExchangeRates.Services;
public class RatesService : IRatesService
{
    private readonly HttpClient _httpClient;
    private readonly string _dateFormat;
    private readonly int _maxRetryCount;

    public RatesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public RatesService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _dateFormat = configuration.GetValue<string>("DefaultDateFormat");
        _maxRetryCount = configuration.GetValue<int>("MaxRetryCount");
    }

    public async Task<ExchangeRateResult> GetExchangeRates(ExchangeRateRequest request)
    {
        var counter = 0;
        var response = await SendRequest(request);
        if (request.StartDate == request.EndDate)
        {
            while (string.IsNullOrWhiteSpace(response) && counter < _maxRetryCount)
            {
                counter++;
                var newRequest = new ExchangeRateRequest(request) { StartDate = request.StartDate.AddDays(-counter), EndDate = request.EndDate.AddDays(-counter) };
                response = await SendRequest(newRequest);
            }
        }

        var rootElement = JObject.Parse(response);
        var rates = rootElement.SelectTokens("$.dataSets..observations.*[0]").Select(t => t.Value<decimal>());
        var dates = rootElement.SelectTokens("$.structure.dimensions.observation[0].values[*].start").Select(t => t.Value<DateTime>());

        return new ExchangeRateResult
        {
            Request = request,
            Data = dates
                .Zip(rates, (date, rate) => new { date, rate })
                .Select(x => new ExchangeRate
                {
                    Date = x.date,
                    Rate = x.rate
                }).ToList()
        };
    }

    private async Task<string> SendRequest(ExchangeRateRequest request)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["startPeriod"] = request.StartDate.ToString(_dateFormat);
        query["endPeriod"] = request.EndDate.ToString(_dateFormat);
        query["format"] = "jsondata";
        var url = $"https://sdw-wsrest.ecb.europa.eu/service/data/EXR/D.{request.SourceCurrencyCode}.{request.TargetCurrencyCode}.SP00.A?{query}";

        var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
        if (!response.IsSuccessStatusCode)
        {
            throw response.StatusCode == System.Net.HttpStatusCode.NotFound
                ? new HttpException(System.Net.HttpStatusCode.NotFound)
                : new HttpException(System.Net.HttpStatusCode.ServiceUnavailable);
        }
        return await response.Content.ReadAsStringAsync();
    }
}

