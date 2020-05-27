using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface ISysFuncPermRepository
    {
        Task<IEnumerable<SysFuncPermItem>> GetSysFuncPermList_V1_0(string roleID);
        Task<bool> HasApiPerm_V1_0(string apiName);
        Task<bool> SaveSysFuncPerm_V1_0(string roleID, IEnumerable<SysFuncPerm> models);
    }
}
