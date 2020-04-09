using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Kernel.Core.Utils
{
    public class ServiceHost
    {
        public static IServiceProvider Provider { get; private set; }

        public static void Load(IServiceProvider provider)
        {
            Provider = provider;
        }

        public static TService GetService<TService>()
        {
            return Provider.GetService<TService>();
        }

        public static IEnumerable<TService> GetServices<TService>()
        {
            return Provider.GetServices<TService>();
        }

    }
}
