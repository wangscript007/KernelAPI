﻿using Kernel.Model.Core;
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
        Task<SysUserLogin> GetSysUserByLoginID_V1_0(string loginID);
        Task<LayuiTable<SysUserListRecord>> GetSysUserList_V1_0(SysUserListIn model);
        Task<SysUser> GetSysUser_V1_0(string userID);
        Task UpdateSysUser_V1_0(SysUser sysUser);
    }
}
