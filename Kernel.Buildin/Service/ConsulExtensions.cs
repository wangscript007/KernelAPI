using Kernel.Core.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class ConsulExtensions
    {
        public static void ConsulRegister(this IConfiguration configuration)
        {
            ConsulHelper.ConsulRegister(configuration);
        }
    }
}