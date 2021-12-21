using ExchangeRates.Models;
using ExchangeRates.Services;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRatesTests;

public class RatesServiceIntegrationTests
{
    [Fact]
    public async Task CheckRatesServiceImplementation()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When("https://sdw-wsrest.ecb.europa.eu/service/data/EXR/D.PLN.EUR.SP00.A?format=jsondata&startPeriod=2021-12-01&endPeriod=2021-12-02")
            .Respond("application/json", File.OpenRead("ExampleResponse.json"));
        var client = mockHttp.ToHttpClient();
        var ratesService = new RatesService(client);

        var request = new ExchangeRateRequest()
        {
            SourceCurrencyCode = "PLN",
            TargetCurrencyCode = "EUR",
            StartDate = new DateTime(2021, 12, 1),
            EndDate = new DateTime(2021, 12, 2)
        };
        var result = await ratesService.GetExchangeRates(request);
        var expectedResult = new List<ExchangeRate>()
        {
            new ExchangeRate() { Date = new DateTime(2021, 12, 1).ToLocalTime().Date, Rate = 4.6283M },
            new ExchangeRate() { Date = new DateTime(2021, 12, 2).ToLocalTime().Date, Rate = 4.5953M },
        };
        Assert.NotNull(result);
        Assert.Equal(expectedResult, result.Data, new ExchangeRateEqualityComparer());
    }

}