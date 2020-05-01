using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.Multitenant
{
    public static class MultitenantExtensions
    {
        public static TenantBuilder AddMultitenancy(this IServiceCollection services)
        {
            return new TenantBuilder(services);
        }

        public static void UseMultiTenant(this IApplicationBuilder app)
        {
            //注册中间件
            //中间件所干的事，很简单，就是捕获进来管道的请求上下文，然后解析得出租户信息，然后把对应的租户信息放入请求上下文中。
            app.Use(next =>
            {
                return async context =>
                {
                    await new MultitenantMiddleware(next).InvokeAsync(context);
                };
            });
        }


    }
}
