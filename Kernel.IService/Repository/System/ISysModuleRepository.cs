﻿using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface ISysModuleRepository
    {
        Task<IEnumerable<T>> GetSysModuleList_V1_0<T>(params string[] modType);
    }
}
