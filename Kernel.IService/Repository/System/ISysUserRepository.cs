using Kernel.Model.Core;
using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface ISysUserRepository
    {
        Task<SysUserLogin> GetSysUserByLoginID_V1_0(string loginID);
        Task<LayuiTable<SysUserListRecord>> GetSysUserList_V1_0(SysUserListIn model);
    }
}
