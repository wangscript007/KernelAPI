using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Core.Multitenant
{
    public interface ITenantStore
    {
        Task<Tenant> GetTenantAsync(string identifier);
    }

    public class InMemoryTenantStore : ITenantStore
    {
        private Tenant[] tenantSource = new[] {
            new Tenant{ ID = "4da254ff2c02488db860cb3b6363c19a", Label = "localhost" }
        };

        public async Task<Tenant> GetTenantAsync(string identifier)
        {
            var tenant = tenantSource.FirstOrDefault(p => p.ID == identifier || p.Label == identifier);
            return await Task.FromResult(tenant);
        }
    }

}
