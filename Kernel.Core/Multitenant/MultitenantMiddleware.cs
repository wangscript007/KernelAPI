using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Core.Multitenant
{
    public class MultitenantMiddleware
    {
        private readonly RequestDelegate _next;

        public MultitenantMiddleware(RequestDelegate next)
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
