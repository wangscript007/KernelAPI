﻿using Kernel.Core.Extensions;
using Kernel.Core.Multitenant;
using Kernel.Core.Utils;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kernel.Core.Models
{
    public class KernelSettings
    {
        public readonly string BasePath;
        public readonly string AttachmentFolder;
        public readonly string AttachmentPath;
        public readonly string ResourcesRootPath;
        public readonly string ResourcesRootFolder;
        public bool IsDevelopment = false;
        [JsonIgnore]
        public readonly IHttpContextAccessor HttpContextAccessor;
        [JsonIgnore]
        public HttpContext HttpContext { get => HttpContextAccessor.HttpContext; }
        public readonly JwtSettings JwtSettings;
        public readonly List<Tenant> Multitenant;
        public Tenant CurrentTenant { get => HttpContext.Items["Tenant"] as Tenant; }
        public string ServerBaseUrl { get => HttpContext.GetServerBaseUrl().EnsureTrailingSlash(); }

        public KernelSettings()
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
            if (!Directory.Exists(AttachmentPath))
                Directory.CreateDirectory(AttachmentPath);

            HttpContextAccessor = ServiceHost.GetService<IHttpContextAccessor>();
            JwtSettings = ServiceHost.GetService<JwtSettings>();
            Multitenant = ServiceHost.GetService<TenantSettings>().MultiTenant;

        }

    }
}
