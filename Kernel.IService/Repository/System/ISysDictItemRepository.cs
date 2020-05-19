using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface ISysDictItemRepository
    {
        Task<Dictionary<string, string>> GetSysDict_V1_0(string dictCode);
    }
}
