### Setup

**NuGet install**:

`Install-Package AspNetCoreRateLimit`

**Startup.cs code**:

```cs
public void ConfigureServices(IServiceCollection services)
{
	// needed to load configuration from appsettings.json
	services.AddOptions();

	// needed to store rate limit counters and ip rules
	services.AddMemoryCache();

	//load general configuration from appsettings.json
	services.Configure<ClientRateLimitOptions>(Configuration.GetSection("ClientRateLimiting"));

	//load client rules from appsettings.json
	services.Configure<ClientRateLimitPolicies>(Configuration.GetSection("ClientRateLimitPolicies"));

	// inject counter and rules stores
	services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
	services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

	// Add framework services.
	services.AddMvc();

        // https://github.com/aspnet/Hosting/issues/793
        // the IHttpContextAccessor service is not registered by default.
        // the clientId/clientIp resolvers use it.
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // configuration (resolvers, counter key builders)
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
	app.UseClientRateLimiting();

	app.UseMvc();
}
```

You should register the middleware before any other components. 

If you load-balance your app, you'll need to use `IDistributedCache` with Redis or SQLServer so that all kestrel instances will have the same rate limit store.
Instead of the in-memory stores you should inject the distributed stores like this:

```cs
// inject counter and rules distributed cache stores
services.AddSingleton<IClientPolicyStore, DistributedCacheClientPolicyStore>();
services.AddSingleton<IRateLimitCounterStore,DistributedCacheRateLimitCounterStore>();
```

**Configuration and general rules appsettings.json**:

```json
  "ClientRateLimiting": {
    "EnableEndpointRateLimiting": false,
    "StackBlockedRequests": false,
    "ClientIdHeader": "X-ClientId",
    "HttpStatusCode": 429,
    "EndpointWhitelist": [ "get:/api/license", "*:/api/status" ],
    "ClientWhitelist": [ "dev-id-1", "dev-id-2" ],
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1s",
        "Limit": 2
      },
      {
        "Endpoint": "*",
        "Period": "15m",
        "Limit": 100
      },
      {
        "Endpoint": "*",
        "Period": "12h",
        "Limit": 1000
      },
      {
        "Endpoint": "*",
        "Period": "7d",
        "Limit": 10000
      }
    ]
  }
```

If `EnableEndpointRateLimiting` is set to `false`, then the limits will apply globally and only the rules that have as endpoint `*` will apply. For example if you set a limit of 5 calls per second, any HTTP call to any endpoint will count towards that limit. 

If `EnableEndpointRateLimiting` is set to `true`, then the limits will apply for each endpoint as in `{HTTP_Verb}{PATH}`. For example if you set a limit of 5 calls per second for `*:/api/values` a client can call `GET /api/values` 5 times per second but also 5 times `PUT /api/values`. 

If `StackBlockedRequests` is set to `false`, rejected calls are not added to the throttle counter. If a client makes 3 requests per second and you've set a limit of one call per second, other limits like per minute or per day counters will only record the first call, the one that wasn't blocked. If you want rejected requests to count towards the other limits, you'll have to set `StackBlockedRequests` to `true`.

The `ClientIdHeader` is used to extract the client id, if a client id is present in this header and matches a value specified in ClientWhitelist then no rate limits are applied. 

**Override general rules for specific clients appsettings.json**:
```json
 "ClientRateLimitPolicies": {
    "ClientRules": [
      {
        "ClientId": "client-id-1",
        "Rules": [
          {
            "Endpoint": "*",
            "Period": "1s",
            "Limit": 10
          },
          {
            "Endpoint": "*",
            "Period": "15m",
            "Limit": 200
          }
        ]
      },
      {
        "ClientId": "client-id-2",
        "Rules": [
          {
            "Endpoint": "*",
            "Period": "1s",
            "Limit": 5
          },
          {
            "Endpoint": "*",
            "Period": "15m",
            "Limit": 150
          },
          {
            "Endpoint": "*",
            "Period": "12h",
            "Limit": 500
          }
        ]
      }
    ]
  }
```

You need to seed these application settings at start-up:

```C#
public static async Task Main(string[] args)
{
    IWebHost webHost = CreateWebHostBuilder(args).Build();

    using (var scope = webHost.Services.CreateScope())
    {
         // get the ClientPolicyStore instance
         var clientPolicyStore = scope.ServiceProvider.GetRequiredService<IClientPolicyStore>();

         // seed client data from appsettings
         await clientPolicyStore.SeedAsync();
    }

    await webHost.RunAsync();
}
```

### Defining rate limit rules

A rule is composed of an endpoint, a period and a limit. 

Endpoint format is `{HTTP_Verb}:{PATH}`, you can target any HTTP verb by using the asterix symbol.

Period format is `{INT}{PERIOD_TYPE}`, you can use one of the following period types: `s, m, h, d`.

Limit format is `{LONG}`.

**Examples**:

Rate limit all endpoints to 2 calls per second: 

Wildcard:
```json
{
 "Endpoint": "*",
 "Period": "1s",
 "Limit": 2
}
```

Regex:
```json
{
 "Endpoint": ".+",
 "Period": "1s",
 "Limit": 2
}
```

If in the same second, a client make 3 GET calls to api/values, the last call will get blocked. But if in the same second he calls PUT api/values too, the request will go through because it's a different endpoint. When endpoint rate limiting is enabled each call is rate limited based on `{HTTP_Verb}{PATH}`. 

Rate limit calls with any HTTP Verb to `/api/values` to 5 calls per 15 minutes: 

Wildcard:
```json
{
 "Endpoint": "*:/api/values",
 "Period": "15m",
 "Limit": 5
}
```

Regex:
```json
{
 "Endpoint": ":/api/values",
 "Period": "15m",
 "Limit": 5
}
```

Rate limit GET call to `/api/values` to 5 calls per hour: 
```json
{
 "Endpoint": "get:/api/values",
 "Period": "1h",
 "Limit": 5
}
```

Rate limit POST or PUT calls to /api/values to 5 calls per hour: 
```json
{
 "Endpoint": "((post)|(put)):/api/values",
 "Period": "1h",
 "Limit": 5
}
```

If in one hour, a client makes 6 GET calls to api/values, the last call will get blocked. But if in the same hour he calls GET api/values/1 too, the request will go through because it's a different endpoint.

### Behavior

When a client make a HTTP call the `ClientRateLimitMiddleware : RateLimitMiddleware<ClientRateLimitProcessor>` does the following: 

- extracts the Client id, HTTP verb and URL from the request object, if you want to implement your own extraction logic you can override the `ClientRateLimitMiddleware.ResolveIdentity` method or implement custom ip/client resolvers:

```C#
public class CustomRateLimitConfiguration : RateLimitConfiguration
{
    protected override void RegisterResolvers()
    {
    	base.RegisterResolvers();

    	ClientResolvers.Add(new ClientQueryStringResolveContributor(HttpContextAccessor, ClientRateLimitOptions.ClientIdHeader));
    }
}
```

- searches for the Client id and URL in the white lists, if any matches then no action is taken
- searches in the Client rules for a match, all rules that apply are grouped by period, for each period the most restrictive rule is used
- searches in the General rules for a match, if a general rule that matches has a defined period that is not present in the Client rules then this general rule is also used
- for each matching rule the rate limit counter is incremented, if the counter value is greater then the rule limit then the request gets blocked

If the request gets blocked then the client receives a text response like this:

```
Status Code: 429
Retry-After: 58
Content: API calls quota exceeded! maximum admitted 2 per 1m.
```

You can customize the response by changing these options `HttpStatusCode` and `QuotaExceededMessage`, if you want to implement your own response you can override the `ClientRateLimitMiddleware.ReturnQuotaExceededResponse`. The `Retry-After` header value is expressed in seconds.

If the request doesn't get rate limited then the longest period defined in the matching rules is used to compose the X-Rate-Limit headers, these headers are injected in the response:

```
X-Rate-Limit-Limit: the rate limit period (eg. 1m, 12h, 1d)
X-Rate-Limit-Remaining: number of request remaining 
X-Rate-Limit-Reset: UTC date time (ISO 8601) when the limits resets
``` 

A client can parse the `X-Rate-Limit-Reset` like this:

```
DateTime resetDate = DateTime.ParseExact(resetHeader, "o", DateTimeFormatInfo.InvariantInfo);
```

The X-Rate-Limit and Retry-After headers can be disabled by setting the `DisableRateLimitHeaders` option to `true` in appsettings.json.

By default blocked request are logged using `Microsoft.Extensions.Logging.ILogger`, if you want to implement your own logging you can override the `ClientRateLimitMiddleware.LogBlockedRequest`.
The default logger emits the following information when a request gets rate limited:

```
info: AspNetCoreRateLimit.ClientRateLimitMiddleware[0]
      Request get:/api/values from ClientId client-id-1 has been blocked, quota 2/1m exceeded by 3. Blocked by rule *:/api/value, TraceIdentifier 0HKTLISQQVV9D.
```

### Update rate limits at runtime

At application startup the client rate limit rules defined in `appsettings.json` are loaded in cache by either `MemoryCacheClientPolicyStore` or `DistributedCacheClientPolicyStore` depending on what type of cache provider you are using. You can access the client policy store inside a controller and modify the rules like so:

```cs
public class ClientRateLimitController : Controller
{
	private readonly ClientRateLimitOptions _options;
	private readonly IClientPolicyStore _clientPolicyStore;

	public ClientRateLimitController(IOptions<ClientRateLimitOptions> optionsAccessor, IClientPolicyStore clientPolicyStore)
	{
		_options = optionsAccessor.Value;
		_clientPolicyStore = clientPolicyStore;
	}

	[HttpGet]
	public ClientRateLimitPolicy Get()
	{
		return _clientPolicyStore.Get($"{_options.ClientPolicyPrefix}_cl-key-1");
	}

	[HttpPost]
	public void Post()
	{
		var id = $"{_options.ClientPolicyPrefix}_cl-key-1";
		var clPolicy = _clientPolicyStore.Get(id);
		clPolicy.Rules.Add(new RateLimitRule
		{
			Endpoint = "*/api/testpolicyupdate",
			Period = "1h",
			Limit = 100
		});
		_clientPolicyStore.Set(id, clPolicy);
	}
}
```

This way you can store the client rate limits in a database and push them in cache after each app start.