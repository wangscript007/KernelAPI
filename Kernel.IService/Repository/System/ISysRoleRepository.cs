using Kernel.Model.Core;
using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface ISysRoleRepository
    {
        Task AddSysRole_V1_0(SysRole model);
        Task<int> DeleteSysRole_V1_0(string[] roleIDs);
        Task<LayuiTableResult<SysRoleListRecord>> GetSysRoleList_V1_0(SysRoleListIn model);
        Task<SysRole> GetSysRole_V1_0(string roleID);
        Task<IEnumerable<string>> GetUserRoles_V1_0(string userID);
        Task UpdateSysRole_V1_0(SysRole model);
    }
}
