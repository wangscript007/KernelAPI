using Kernel.Core.Multitenant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Service.Core
{
    public class HeaderTenantResolver : ITenantResolver
    {
        private readonly IHttpContextAccessor _accessor;

        public HeaderTenantResolver(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        // 这里就解析道了具体的域名了，从而就能得知当前租户
        public async Task<string> GetTenantIdentifierAsync()
        {
            return await Task.FromResult(_accessor.HttpContext.Request.Headers["Tenant"]);
        }

    }
}
