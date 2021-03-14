using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Env
    {
        public static readonly string KERNEL_ENVIRONMENT;
        public static readonly string KERNEL_CONFIG_CENTER;
        public static readonly string KERNEL_SERVICE_NAME;

        static Env()
        {
            KERNEL_ENVIRONMENT = KernelApp.GetEnv("KERNEL_ENVIRONMENT");
            KERNEL_CONFIG_CENTER = KernelApp.GetEnv("KERNEL_CONFIG_CENTER");
            KERNEL_SERVICE_NAME = KernelApp.GetEnv("KERNEL_SERVICE_NAME");

            KernelApp.Log.Info("KERNEL_ENVIRONMENT:" + KERNEL_ENVIRONMENT);
            KernelApp.Log.Info("KERNEL_CONFIG_CENTER:" + KERNEL_CONFIG_CENTER);
            KernelApp.Log.Info("KERNEL_SERVICE_NAME:" + KERNEL_SERVICE_NAME);
        }
    }
}
