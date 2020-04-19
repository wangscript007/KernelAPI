using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.Utils
{
    using log4net;
    using log4net.Config;
    using log4net.Repository;
    using System.IO;

    public class LogHelper
    {
        private static ILoggerRepository repository { get; set; }
        private static ILog _log;
        public static ILog log
        {
            get
            {
                if (_log == null)
                {
                    Configure();
                }
                return _log;
            }
        }

        public static void Configure(string repositoryName = "NETCoreRepository", string configFile = "Settings\\log4net.config")
        {
            repository = LogManager.CreateRepository(repositoryName);
            XmlConfigurator.Configure(repository, new FileInfo(configFile));
            _log = LogManager.GetLogger(repositoryName, "");
        }
    }

}
