using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Service.System
{
    public interface ISysModuleService
    {
        Task<IEnumerable<SysPermTree>> GetPermTree();
        Task<SysModuleInit> GetSysModuleInit();
    }
}
