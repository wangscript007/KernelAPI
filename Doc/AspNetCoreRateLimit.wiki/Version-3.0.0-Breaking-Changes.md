### The changes are explained in more details in [this pull request](https://github.com/stefanprodan/AspNetCoreRateLimit/pull/63)

---

- The target framework has been updated to `netstandard2.0`.

- We have to manually seed the `appsettings.json` policies:

```C#
public static async Task Main(string[] args)
{
    IWebHost webHost = CreateWebHostBuilder(args).Build();

    using (var scope = webHost.Services.CreateScope())
    {
         // get the ClientPolicyStore instance
         var clientPolicyStore = scope.ServiceProvider.GetRequiredService<IClientPolicyStore>();

         // seed Client data from appsettings
         await clientPolicyStore.SeedAsync();

         // get the IpPolicyStore instance
         var ipPolicyStore = scope.ServiceProvider.GetRequiredService<IIpPolicyStore>();

         // seed IP data from appsettings
         await ipPolicyStore.SeedAsync();
    }

    await webHost.RunAsync();
}
```

- We have to register the rate limit configuration in `Startup.cs`:

```C#
// configure the resolvers
services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
```