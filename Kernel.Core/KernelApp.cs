using Kernel.Core.Models;
using Kernel.Core.Utils;
using log4net;
using System.Runtime.InteropServices;

namespace System
{
    public static class KernelApp
    {
        private static object _lockedObj = new object();

        private static KernelSettings settings;
        private static KernelRequest request;

        public static KernelSettings Settings
        {
            get
            {
                lock (_lockedObj)
                {
                    if (settings == null)
                    {
                        settings = new KernelSettings();
                    }
                }
                return settings;
            }
        }

        public static KernelRequest Request
        {
            get
            {
                lock (_lockedObj)
                {
                    if (request == null)
                    {
                        request = new KernelRequest();
                    }
                }
                return request;
            }
        }

        public static ILog Log { get => LogHelper.Log; }

        public static string GetEnv(string key)
        {
            //return Environment.GetEnvironmentVariable(key, RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? EnvironmentVariableTarget.Machine : EnvironmentVariableTarget.Process);
            //linux上只支持EnvironmentVariableTarget.Process

            string log = $"读取环境变量{key}：";
            Console.WriteLine(log);

            string value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
            log = "from Process,value:" + value;
            Console.WriteLine(log);

            if (string.IsNullOrWhiteSpace(value))
            {
                value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);
                log = "from User,value:" + value;
                Console.WriteLine(log);
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
                log = "from Machine,value:" + value;
                Console.WriteLine(log);
            }

            return value;
        }

    }

}
