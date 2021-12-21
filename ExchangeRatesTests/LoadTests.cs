using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRatesTests;

public class LoadTests
{
    [Fact]
    public async Task Load()
    {
        var stopwatch = new Stopwatch();
        var httpClient = new HttpClient();
        var request = CreateSampleRequest();

        stopwatch.Start();
        var response = await httpClient.SendAsync(request);
        stopwatch.Stop();
        var firstRequestMilliseconds = stopwatch.ElapsedMilliseconds;
        
        request = CreateSampleRequest();
        stopwatch.Restart();
        await httpClient.SendAsync(request);
        stopwatch.Stop();
        var secondRequestMilliseconds = stopwatch.ElapsedMilliseconds;

        Console.WriteLine($"{firstRequestMilliseconds}ms, {secondRequestMilliseconds}ms");
        Assert.True(firstRequestMilliseconds > secondRequestMilliseconds);
    }

    private HttpRequestMessage CreateSampleRequest()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7153/api/rates?startDate=2021-01-01&endDate=2021-12-01&apiKey=rfIHsYPyD0IiOMoyILth7qhBnnGQlOghlFXChNgj6FJqPoJD6suyrrJlLKHCuHYA");
        var currencyPairs = new Dictionary<string, string>
        {
            { "PLN", "EUR" },
            { "USD", "EUR" },
            { "GBP", "EUR" },
            { "CZK", "EUR" },
            { "NOK", "EUR" },
            { "JPY", "EUR" }
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(currencyPairs));
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        return request;
    }
}