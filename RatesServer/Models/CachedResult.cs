using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExchangeRates.Models;

public class CachedResult : ExchangeRateResult
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public CachedResult() { }
    public CachedResult(ExchangeRateResult other)
    {
        Request = other.Request;
        Data = other.Data;
    }
}
