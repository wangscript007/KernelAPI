using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface ISysRoleRepository
    {
        Task<IEnumerable<string>> GetUserRoles_V1_0(string userID);
    }
}
