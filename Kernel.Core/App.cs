using Kernel.Core.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kernel.Core
{
    public static class App
    {
        public static readonly string BasePath;
        public static readonly string AttachmentFolder;
        public static readonly string AttachmentPath;
        public static readonly string ResourcesRootPath;
        public static readonly string ResourcesRootFolder;
        public static bool IsDevelopment = false;
        public static readonly IHttpContextAccessor HttpContextAccessor;
        public static HttpContext HttpContext { get => HttpContextAccessor.HttpContext; }

        static App()
        {
            //BasePath = PlatformServices.Default.Application.ApplicationBasePath;
            BasePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            ResourcesRootFolder = AppsettingsConfig.GetConfigValue("App:ResourcesRootFolder");
            var resourcesPath = AppsettingsConfig.GetConfigValue("App:ResourcesPath");
            if (resourcesPath == "")
                ResourcesRootPath = BasePath + ResourcesRootFolder + Path.DirectorySeparatorChar;
            else
                ResourcesRootPath = resourcesPath + ResourcesRootFolder + Path.DirectorySeparatorChar;

            AttachmentFolder = AppsettingsConfig.GetConfigValue("FileUpload:AttachmentFolder");
            AttachmentPath = ResourcesRootPath + AttachmentFolder + Path.DirectorySeparatorChar;
            //创建文件下载路径
            if (!Directory.Exists(App.AttachmentPath))
                Directory.CreateDirectory(App.AttachmentPath);

            HttpContextAccessor = ServiceHost.GetService<IHttpContextAccessor>();
        }
    }
}
