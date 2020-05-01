using Kernel.Core.Multitenant;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Service.Core
{
    public class TenantResolver : ITenantResolver
    {
        private readonly IHttpContextAccessor _accessor;

        public TenantResolver(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task<string> GetTenantIdentifierAsync()
        {
            //优先从token里面取
            var tenantID = _accessor.HttpContext.User.Claims.FirstOrDefault(o => o.Type == "Tenant")?.Value;
            if (tenantID != null)
                return tenantID;

            tenantID = _accessor.HttpContext.Request.Headers["TenantID"];
            if (tenantID != null)
                return tenantID;

            tenantID = _accessor.HttpContext.Request.Query["TenantID"];

            return await Task.FromResult(tenantID);
        }

    }
}
