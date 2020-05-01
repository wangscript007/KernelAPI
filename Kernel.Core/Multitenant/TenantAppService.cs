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
            return await _tenantStore.GetTenantAsync(identifier);
        }
    }
}
