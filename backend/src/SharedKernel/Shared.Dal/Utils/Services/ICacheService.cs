using Shared.Abstractions.Serialization;
using StackExchange.Redis;

namespace Shared.Dal.Utils.Services;

public interface ICacheService
{
    ValueTask<string> GetAsync(string key);
    ValueTask SetData<TEntity>(string key, TEntity entity, TimeSpan expiry);
}

public class CacheService : ICacheService
{
    private IConnectionMultiplexer _redis;
    private readonly IJsonSerializer _jsonSerializer;

    public CacheService(
        IConnectionMultiplexer redis,
        IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;

        _redis = redis;
    }

    public async ValueTask<string> GetAsync(string key)
    {
        var value = await _redis.GetDatabase().StringGetAsync(key);

        return string.IsNullOrEmpty(value) ? null : value.ToString();
    }

    public async ValueTask SetData<TEntity>(string key, TEntity entity, TimeSpan expiry)
        => await _redis.GetDatabase().StringSetAsync(key, _jsonSerializer.Serialize(entity), expiry);
}
