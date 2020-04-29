using Kernel.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.MyTenant
{
    public class Tenant
    {
        public string Identifier { get; set; }

        public string Id { get; set; }

    }

    public interface ITenantResolver
    {
        Task<string> GetTenantIdentifierAsync();
    }

    public class DomainTenantResolver : ITenantResolver
    {
        private readonly IHttpContextAccessor _accessor;

        public DomainTenantResolver(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        // 这里就解析道了具体的域名了，从而就能得知当前租户
        public async Task<string> GetTenantIdentifierAsync()
        {
            return await Task.FromResult(_accessor.HttpContext.Request.Host.Host);
        }

    }

    public interface ITenantStore
    {
        Task<Tenant> GetTenantAsync(string identifier);
    }

    public class InMemoryTenantStore : ITenantStore
    {
        private Tenant[] tenantSource = new[] {
            new Tenant{ Id = "4da254ff-2c02-488d-b860-cb3b6363c19a", Identifier = "localhost" }
        };

        public async Task<Tenant> GetTenantAsync(string identifier)
        {
            var tenant = tenantSource.FirstOrDefault(p => p.Identifier == identifier);
            return await Task.FromResult(tenant);
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static TenantBuilder AddMultiTenancy(this IServiceCollection services)
        {
            return new TenantBuilder(services);
        }
    }

    public class TenantBuilder
    {
        private readonly IServiceCollection _services;
        public TenantBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public TenantBuilder WithTenantResolver<TIml>(ServiceLifetime lifttime = ServiceLifetime.Transient) where TIml : ITenantResolver
        {
            _services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();  // 这一步很重要
            _services.AddScoped(typeof(ITenantResolver), typeof(TIml));
            return this;
        }

        public TenantBuilder WithTenantStore<TIml>(ServiceLifetime lifttime = ServiceLifetime.Transient)
        {
            _services.AddScoped(typeof(ITenantStore), typeof(TIml));
            return this;
        }
    }

    public class TenantAppService
    {
        private readonly ITenantResolver _tenantResolver;
        private readonly ITenantStore _tenantStore;

        public TenantAppService(ITenantResolver tenantResolver, ITenantStore tenantStore)
        {
            _tenantResolver = tenantResolver;
            _tenantStore = tenantStore;
        }

        public async Task<Tenant> GetTenantAsync()
        {
            var identifier = await _tenantResolver.GetTenantIdentifierAsync();
            return await _tenantStore.GetTenantAsync(identifier);
        }
    }

    class MultiTenantMiddleware
    {
        private readonly RequestDelegate _next;

        public MultiTenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Items.ContainsKey("Tenant"))
            {
                //var tenantAppService = ServiceHost.GetService<TenantAppService>();
                var tenantAppService = context.RequestServices.GetService(typeof(TenantAppService)) as TenantAppService;

                // 这里也可以放到其他地方，比如 context.User.Cliams 中
                context.Items.Add("Tenant", await tenantAppService.GetTenantAsync());
            }

            if (_next != null)
                await _next(context);
        }




    }

}
