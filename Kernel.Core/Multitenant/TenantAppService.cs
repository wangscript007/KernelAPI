using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Core.Multitenant
{
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
            if (string.IsNullOrEmpty(identifier))
                return null;

            var tenant = await _tenantStore.GetTenantAsync(identifier);
            if (tenant == null)
                return new Tenant { Label = identifier };//返回匿名租户

            return tenant;
        }
    }
}
