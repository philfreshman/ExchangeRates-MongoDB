using MongoDB.Bson.Serialization.Attributes;

namespace ExchangeRates.Models;
public class ExchangeRateRequest
{
    public string SourceCurrencyCode { get; set; }

    public string TargetCurrencyCode { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public ExchangeRateRequest() { }
    public ExchangeRateRequest(ExchangeRateRequest request)
    {
        SourceCurrencyCode = request.SourceCurrencyCode;
        TargetCurrencyCode = request.TargetCurrencyCode;
        StartDate = request.StartDate;
        EndDate = request.EndDate;
    }
}