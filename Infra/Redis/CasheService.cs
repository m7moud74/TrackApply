using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

public class CahseService(IDistributedCache cache) : ICasheService
{
    public async Task<T> GetTAsync<T>(string Key, CancellationToken cancellationToken = default)
    {
        var CasheValue = await cache.GetStringAsync(Key, cancellationToken);
        if (cache is null)
            return default!;
        return JsonSerializer.Deserialize<T>(CasheValue!)!;
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(key, cancellationToken);
    }

    public async Task  SetTAsync<T>(T value, string key, TimeSpan? expiretime = null, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions();
        
        if (expiretime.HasValue)
        {
            options.AbsoluteExpirationRelativeToNow = expiretime;
        }

        var serializedValue = JsonSerializer.Serialize(value);
        
        await cache.SetStringAsync(key, serializedValue, options, cancellationToken);
    }
}