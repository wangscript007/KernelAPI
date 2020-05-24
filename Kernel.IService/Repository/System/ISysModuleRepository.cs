using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface ISysModuleRepository
    {
        Task<SysModule> GetSysModuleByNavUrl_V1_0(string navUrl);
        Task<IEnumerable<T>> GetSysModuleList_V1_0<T>(string modType);
    }
}
