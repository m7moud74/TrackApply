public interface ICacheService
{
    Task<T> GetTAsync<T>(string Key, CancellationToken cancellationToken = default);
    Task SetTAsync<T>(T value, string key, TimeSpan? expiretime = null, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}