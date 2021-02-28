You can implement you own custom resolver contributors (for the client id and/or IP) via the `RateLimitConfiguration` class:

```C#
public class RateLimitConfiguration : IRateLimitConfiguration
{
        
    protected virtual void RegisterResolvers()
    {
        if (!string.IsNullOrEmpty(ClientRateLimitOptions?.ClientIdHeader))
        {
            ClientResolvers.Add(new ClientHeaderResolveContributor(HttpContextAccessor, ClientRateLimitOptions.ClientIdHeader));
        }

        // the contributors are resolved in the order of their collection index
        if (!string.IsNullOrEmpty(IpRateLimitOptions?.RealIpHeader))
        {
                IpResolvers.Add(new IpHeaderResolveContributor(HttpContextAccessor, IpRateLimitOptions.RealIpHeader));
        }

        IpResolvers.Add(new IpConnectionResolveContributor(HttpContextAccessor));
    }
}
```

There are some predefined resolve contributors:

- `IpConnectionResolveContributor` - gets the IP from the `HttpContext.Connection.RemoteIpAddress` property

- `IpHeaderResolveContributor` - gets the IP from the HTTP header named via the `RealIpHeader` application setting - can be used if the application is hosted behind a proxy

- `ClientHeaderResolveContributor` - gets the Client Id from the HTTP header named via the `ClientIdHeader` application setting

You can define your own contributor by implementing the `IClientResolveContributor` interface (that for example parses the query string to get the Client Id) and add it to the configuration:

```C#
public class CustomRateLimitConfiguration : RateLimitConfiguration
{
    protected override void RegisterResolvers()
    {
    	base.RegisterResolvers();

    	ClientResolvers.Add(new ClientQueryStringResolveContributor(HttpContextAccessor, "queryStringParamName"));
    }
}
```

Then, you just need to add it to the dependency container:

```C#
services.AddSingleton<IRateLimitConfiguration, CustomRateLimitConfiguration>();
```