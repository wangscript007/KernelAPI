If you load-balance your app, you'll need to use `IDistributedCache` with Redis or SQLServer so that all app instances will have the same rate limit counter store.

* Before going down this path you should know that there's a concurrency issue being discussed here: https://github.com/stefanprodan/AspNetCoreRateLimit/issues/83

## Solution 1

- Use the distributed implementations in `Startup.cs`:
```C#
// inject counter and rules distributed cache stores
services.AddSingleton<IClientPolicyStore, DistributedCacheClientPolicyStore>();
services.AddSingleton<IRateLimitCounterStore,DistributedCacheRateLimitCounterStore>();
```
- Install the [Microsoft.Extensions.Caching.StackExchangeRedis](https://www.nuget.org/packages/Microsoft.Extensions.Caching.StackExchangeRedis/3.0.0) NuGet package
- Setup the Redis connection:
```C#
services.AddStackExchangeRedisCache(options =>
{
    options.ConfigurationOptions = new ConfigurationOptions
    {
        //silently retry in the background if the Redis connection is temporarily down
        AbortOnConnectFail = false
    };
    options.Configuration = "localhost:6379";
    options.InstanceName = "AspNetRateLimit";
});
```

## Solution 2 - in-memory cache fallback

- Implement the `IRateLimitCounterStore` interface:
```C#
public class RedisRateLimitCounterStore : IRateLimitCounterStore
{
    private readonly ILogger _logger;
    private readonly IRateLimitCounterStore _memoryCacheStore;
    private readonly RedisOptions _redisOptions;
    private readonly ConnectionMultiplexer _redis;

    public RedisRateLimitCounterStore(
        IOptions<RedisOptions> redisOptions,
        IMemoryCache memoryCache,
        ILogger<RedisRateLimitCounterStore> logger)
    {
        _logger = logger;
        _memoryCacheStore = new MemoryCacheRateLimitCounterStore(memoryCache);

        _redisOptions = redisOptions?.Value;
        _redis = ConnectionMultiplexer.Connect(_redisOptions.ConnectionString);
    }

    private IDatabase RedisDatabase => _redis.GetDatabase();

    public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await TryRedisCommandAsync(
            () =>
            {
                return RedisDatabase.KeyExistsAsync(id);
            },
            () =>
            {
                return _memoryCacheStore.ExistsAsync(id, cancellationToken);
            });
    }

    public async Task<RateLimitCounter?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await TryRedisCommandAsync(
            async () =>
            {
                var value = await RedisDatabase.StringGetAsync(id);

                if (!string.IsNullOrEmpty(value))
                {
                    return JsonConvert.DeserializeObject<RateLimitCounter?>(value);
                }

                return null;
            },
            () =>
            {
                return _memoryCacheStore.GetAsync(id, cancellationToken);
            });
    }

    public async Task RemoveAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = await TryRedisCommandAsync(
            async () =>
            {
                await RedisDatabase.KeyDeleteAsync(id);

                return true;
            },
            async () =>
            {
                await _memoryCacheStore.RemoveAsync(id, cancellationToken);

                return true;
            });
    }

    public async Task SetAsync(string id, RateLimitCounter? entry, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = await TryRedisCommandAsync(
            async () =>
            {
                await RedisDatabase.StringSetAsync(id, JsonConvert.SerializeObject(entry.Value), expirationTime);

                return true;
            },
            async () =>
            {
                await _memoryCacheStore.SetAsync(id, entry, expirationTime, cancellationToken);

                return true;
            });
    }

    private async Task<T> TryRedisCommandAsync<T>(Func<Task<T>> command, Func<Task<T>> fallbackCommand)
    {
        if (_redisOptions?.Enabled == true && _redis?.IsConnected == true)
        {
            try
            {
                return await command();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Redis command failed: {ex}");
            }
        }

        return await fallbackCommand();
    }
}

```
- Inject the distributed implementations:
```C#
// inject counter and rules distributed cache stores
services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
services.AddSingleton<IRateLimitCounterStore, RedisRateLimitCounterStore>();
```
Note: if you have dynamic client policies (new policies at runtime), you also need to implement the `IClientPolicyStore` if you want to use the same Redis `ConnectionMultiplexer`. Else you can use the `MemoryCacheClientPolicyStore` or the `DistributedCacheClientPolicyStore` configured as the example in **Solution 1**