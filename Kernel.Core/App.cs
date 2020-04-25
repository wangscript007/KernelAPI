using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core
{
    public static class App
    {
        public static readonly string BasePath;

        static App()
        {
            BasePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }
    }
}
