using Microsoft.Extensions.Configuration;
using System.IO;

namespace Kernel.Core.Utils
{
    public static class AppsettingsConfig
    {
        public static IConfiguration Configuration { get; set; }

        static AppsettingsConfig()
        {
            Configuration = ServiceHost.GetService<IConfiguration>();
        }

        public static string GetConfigValue(string key)
        {
            return Configuration[key];
        }
    }
}
