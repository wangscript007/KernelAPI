# AspNetCoreRateLimit

AspNetCoreRateLimit is an ASP.NET Core rate limiting solution designed to control the rate of requests that clients can make to a Web API or MVC app based on IP address or client ID. The AspNetCoreRateLimit [NuGet package](https://www.nuget.org/packages/AspNetCoreRateLimit/) contains an **IpRateLimitMiddleware** and a **ClientRateLimitMiddleware**, with each middleware you can set multiple limits for different scenarios like allowing an IP or Client to make a maximum number of calls in a time interval like per second, 15 minutes, etc. You can define these limits to address all requests made to an API or you can scope the limits to each API URL or HTTP verb and path.

AspNetCoreRateLimit targets both `netstandard2.0` & `netcoreapp3.1`. The package has the following dependencies:

```xml
.NETCoreApp 3.1
Microsoft.Extensions.Caching.Abstractions (>= 3.1.9)
Microsoft.Extensions.Logging.Abstractions (>= 3.1.9)
Microsoft.Extensions.Options (>= 3.1.9)
Newtonsoft.Json (>= 12.0.3)
```
```xml
.NETStandard 2.0
Microsoft.AspNetCore.Http.Abstractions (>= 2.2.0)
Microsoft.Extensions.Caching.Abstractions (>= 2.2.0)
Microsoft.Extensions.Logging.Abstractions (>= 2.2.0)
Microsoft.Extensions.Options (>= 2.2.0)
Newtonsoft.Json (>= 12.0.3)
```

```xml
.NET 5.0
Microsoft.Extensions.Caching.Abstractions (>= 5.0.0)
Microsoft.Extensions.Logging.Abstractions (>= 5.0.0)
Microsoft.Extensions.Options (>= 5.0.0)
Newtonsoft.Json (>= 12.0.3)
```

[Version 3.0.0 Breaking Changes](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/Version-3.0.0-Breaking-Changes)

### Rate limiting based on client IP

- [Setup and configuration](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#setup)
- [Defining rate limit rules](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#defining-rate-limit-rules)
- [Behavior](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#behavior)
- [Update rate limits at runtime](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#update-rate-limits-at-runtime)

### Rate limiting based on client ID

- [Setup and configuration](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/ClientRateLimitMiddleware#setup)
- [Defining rate limit rules](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/ClientRateLimitMiddleware#defining-rate-limit-rules)
- [Behavior](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/ClientRateLimitMiddleware#behavior)
- [Update rate limits at runtime](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/ClientRateLimitMiddleware#update-rate-limits-at-runtime)

### Advanced configuration

- [IP / ClientId Resolve Contributors](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/Resolve-Contributors)
- [Quota Exceeded Response Customization](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/Quota-exceeded-response)
- [Using Redis as a distributed counter store](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/Using-Redis-as-a-distributed-counter-store)