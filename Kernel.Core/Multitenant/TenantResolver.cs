using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Core.Multitenant
{

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

        // 这里就解析到了具体的域名了，从而就能得知当前租户
        public async Task<string> GetTenantIdentifierAsync()
        {
            return await Task.FromResult(_accessor.HttpContext.Request.Host.Host);
        }

    }
}
