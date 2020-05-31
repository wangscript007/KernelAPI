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
        Task AddSysUser_V1_0(SysUser sysUser);
        Task<int> DeleteSysUser_V1_0(string[] userIDs);
        Task<SysUserLogin> GetSysUserByLoginID_V1_0(string loginID);
        Task<LayuiTableResult<SysUserListRecord>> GetSysUserList_V1_0(SysUserListIn model);
        Task<SysUser> GetSysUser_V1_0(string userID);
        Task<int> UpdatePwd_V1_0(SysUser model);
        Task UpdateSysUser_V1_0(SysUser sysUser);
    }
}
