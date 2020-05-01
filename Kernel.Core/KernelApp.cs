using Kernel.Core.Models;

namespace System
{
    public static class KernelApp
    {
        private static object _lockedObj = new object();

        private static KernelSettings settings;
        public static KernelSettings Settings
        {
            get 
            {
                lock(_lockedObj)
                {
                    if (settings == null)
                    {
                        settings = new KernelSettings();
                    }
                }
                return settings;
            }
        }
    }

}
