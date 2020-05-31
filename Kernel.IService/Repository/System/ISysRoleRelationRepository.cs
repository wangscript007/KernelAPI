using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface ISysRoleRelationRepository
    {
        Task<SysRoleRelationSet> GetSysRoleRelation_V1_0(string userID);
        Task<bool> SaveSysRoleRelation_V1_0(SysRoleRelationSaveIn model);
    }
}
