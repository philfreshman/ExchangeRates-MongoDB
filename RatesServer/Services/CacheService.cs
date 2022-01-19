using ExchangeRates.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ExchangeRates.Services;
public class CacheService : ICacheService
{
    private readonly IMongoCollection<CachedResult> _cachedResults;

    public CacheService(IOptions<ExchangeRatesDatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _cachedResults = mongoDb.GetCollection<CachedResult>(dbSettings.Value.CollectionName);
    }

    public async Task<ExchangeRateResult> GetCachedResult(ExchangeRateRequest request)
    {
        var result = await _cachedResults.Find(r => r.Request.Equals(request)).FirstOrDefaultAsync();
        return result ?? new ExchangeRateResult(request);
    }

    public async Task UpdateCachedResult(ExchangeRateResult result)
    {
        var existingResult = await _cachedResults.Find(r => r.Request.Equals(result.Request)).FirstOrDefaultAsync();
        if (existingResult == null)
        {
            await _cachedResults.InsertOneAsync(new CachedResult(result));
        }
        else
        {
            await _cachedResults.ReplaceOneAsync(x => x.Id == existingResult.Id, new CachedResult(result));
        }
    }

    public Task BulkUpdateCachedResults(IEnumerable<ExchangeRateResult> results)
    {
        var tasks = results.Select(r => UpdateCachedResult(r));
        return Task.WhenAll(tasks);
    }
}