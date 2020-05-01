using Kernel.Core.Multitenant;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Repository.Core
{
    public class JsonFileTenantStore : ITenantStore
    {
        public async Task<Tenant> GetTenantAsync(string identifier)
        {
            var tenant = KernelApp.Settings.Multitenant.FirstOrDefault(p => p.ID == identifier);
            return await Task.FromResult(tenant);
        }
    }
}
