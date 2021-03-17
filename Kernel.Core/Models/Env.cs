using ConsoleTables;
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
            ColorConsole.WriteWrappedHeader("读取环境变量", headerColor: ConsoleColor.Green);

            KERNEL_ENVIRONMENT = KernelApp.GetEnv("KERNEL_ENVIRONMENT");
            KERNEL_CONFIG_CENTER = KernelApp.GetEnv("KERNEL_CONFIG_CENTER");
            KERNEL_SERVICE_NAME = KernelApp.GetEnv("KERNEL_SERVICE_NAME");

            var table = new ConsoleTable("KEY", "VALUE");
            table.AddRow("KERNEL_ENVIRONMENT", KERNEL_ENVIRONMENT)
                 .AddRow("KERNEL_CONFIG_CENTER", KERNEL_CONFIG_CENTER)
                 .AddRow("KERNEL_SERVICE_NAME", KERNEL_SERVICE_NAME);

            table.Write();

            KernelApp.Log.Info("KERNEL_ENVIRONMENT:" + KERNEL_ENVIRONMENT);
            KernelApp.Log.Info("KERNEL_CONFIG_CENTER:" + KERNEL_CONFIG_CENTER);
            KernelApp.Log.Info("KERNEL_SERVICE_NAME:" + KERNEL_SERVICE_NAME);
        }
    }
}
