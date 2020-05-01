using Kernel.Core;
using Kernel.Core.Multitenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Repository.Core
{
    public class JsonFileTenantStore : ITenantStore
    {
        public async Task<Tenant> GetTenantAsync(string identifier)
        {
            var tenant = App.Multitenant.FirstOrDefault(p => p.ID == identifier);
            return await Task.FromResult(tenant);
        }
    }
}
