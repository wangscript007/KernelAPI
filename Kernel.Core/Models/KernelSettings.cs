using Kernel.Core.Extensions;
using Kernel.Core.Multitenant;
using Kernel.Core.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

namespace Kernel.Core.Models
{
    public class KernelSettings
    {
        public readonly string BasePath;
        public readonly string AttachmentFolder;
        public readonly string AttachmentPath;
        public readonly string ResourcesRootPath;
        public readonly string ResourcesRootFolder;
        [JsonIgnore]
        public readonly IHostingEnvironment Env;
        public bool IsDevelopment { get => Env.IsDevelopment(); }
        [JsonIgnore]
        public readonly IHttpContextAccessor HttpContextAccessor;

        public readonly JwtSettings JwtSettings;
        public readonly List<Tenant> Multitenant;
        [JsonIgnore]
        public readonly IConfiguration Config;
        public string NewGUID { get => Guid.NewGuid().ToString("N"); }
        public readonly string EnabledActionLog;


        public KernelSettings()
        {
            //BasePath = PlatformServices.Default.Application.ApplicationBasePath;
            BasePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            ResourcesRootFolder = AppsettingsConfig.GetConfigValue("App:ResourcesRootFolder");
            var resourcesPath = AppsettingsConfig.GetConfigValue("App:ResourcesPath");
            if (resourcesPath == "")
                ResourcesRootPath = BasePath + ResourcesRootFolder;
            else
                ResourcesRootPath = resourcesPath + ResourcesRootFolder;

            AttachmentFolder = AppsettingsConfig.GetConfigValue("FileUpload:AttachmentFolder");
            AttachmentPath = ResourcesRootPath + Path.DirectorySeparatorChar + AttachmentFolder + Path.DirectorySeparatorChar;
            //创建文件下载路径
            if (!Directory.Exists(AttachmentPath))
                Directory.CreateDirectory(AttachmentPath);

            HttpContextAccessor = ServiceHost.GetService<IHttpContextAccessor>();
            JwtSettings = ServiceHost.GetService<JwtSettings>();
            Multitenant = ServiceHost.GetService<TenantSettings>().MultiTenant;
            Env = ServiceHost.GetService<IHostingEnvironment>();
            Config = ServiceHost.GetService<IConfiguration>();

            EnabledActionLog = AppsettingsConfig.GetConfigValue("EnabledActionLog");

        }

    }
}
