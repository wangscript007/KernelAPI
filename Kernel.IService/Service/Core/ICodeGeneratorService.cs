using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.IService.Service.Core
{
    public interface ICodeGeneratorService
    {
        string Generation(string tableName);
    }
}
