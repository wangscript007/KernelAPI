﻿using Kernel.Core.Models;
using Kernel.Core.Utils;
using log4net;

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

    }

}
