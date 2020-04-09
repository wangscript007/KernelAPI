using Kernel.Model.Demo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.Demo
{
    public interface IUserRepository
    {
        Task<SysUser> GetUserInfo_V1_0(SysUserInParams model);
        Task<SysUserExt1> GetUserInfo_V2_0(SysUserInParams model);
        Task<IEnumerable<SysUserExt2>> GetUserList_V1_0(SysUserInParams model);
    }
}
