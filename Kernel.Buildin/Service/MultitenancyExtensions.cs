using Kernel.Core.Multitenant;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class MultitenancyExtensions
    {
        public static void AddBuildinMultitenancy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMultitenancy()
                //.WithTenantResolver<HeaderTenantResolver>()
                //.WithTenantStore<JsonFileTenantStore>()
                .WithTenantService(configuration);
        }

        public static void AddBuildinMultitenancy(this IApplicationBuilder app)
        {
            app.UseMultiTenant();

        }


    }
}
