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
        private const string DEFAULT_REPO_NAME = "NETCoreRepository";
        private const string DEFAULT_LOGGER_NAME = "";

        public static ILoggerRepository Repository { get; set; }
        public static ILog Log { get; set; }

        static LogHelper()
        {
            Repository = Configure();
            Log = GetLogger(Repository.Name);
        }
        
        public static ILoggerRepository Configure(string repositoryName = DEFAULT_REPO_NAME, string configFile = null)
        {
            var repository = LogManager.CreateRepository(repositoryName);
            configFile = configFile ?? AppDomain.CurrentDomain.SetupInformation.ApplicationBase + $"Settings.{Env.ASPNETCORE_ENVIRONMENT}{Path.DirectorySeparatorChar}Log4net.config";
            XmlConfigurator.Configure(repository, new FileInfo(configFile));
            return repository;
        }

        public static ILoggerRepository GetRepository(string repositoryName = DEFAULT_REPO_NAME)
        {
            return LogManager.GetRepository(repositoryName);
        }

        public static ILog GetLogger(string repositoryName = DEFAULT_REPO_NAME, string name = DEFAULT_LOGGER_NAME)
        {
            return LogManager.GetLogger(repositoryName, name);
        }
    }

}
