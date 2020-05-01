using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.Multitenant
{
    public class TenantBuilder
    {
        private readonly IServiceCollection _services;
        public TenantBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public TenantBuilder WithTenantResolver<TIml>() where TIml : ITenantResolver
        {
            _services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();  // 这一步很重要
            _services.AddScoped(typeof(ITenantResolver), typeof(TIml));
            return this;
        }

        public TenantBuilder WithTenantStore<TIml>() where TIml : ITenantStore
        {
            _services.AddScoped(typeof(ITenantStore), typeof(TIml));
            return this;
        }

        public TenantBuilder WithTenantService(IConfiguration configuration)
        {
            var settings = new TenantSettings();
            configuration.GetSection("Multitenant").Bind(settings.MultiTenant);
            _services.AddSingleton(settings);

            _services.AddScoped(typeof(TenantAppService));
            return this;
        }
    }
}
