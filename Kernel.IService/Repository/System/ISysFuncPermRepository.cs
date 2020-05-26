using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface ISysFuncPermRepository
    {
        Task<bool> HasApiPerm_V1_0(string apiName);
    }
}
