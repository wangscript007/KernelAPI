using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface ISysMenuPermRepository
    {
        Task<bool> SaveSysMenuPerm_V1_0(string roleID, IEnumerable<SysMenuPerm> models);
    }
}
