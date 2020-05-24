using Kernel.Core.Extensions;
using Kernel.Core.Multitenant;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Kernel.Core.Models
{
    public class KernelRequest
    {
        [JsonIgnore]
        public HttpContext HttpContext { get => KernelApp.Settings.HttpContextAccessor.HttpContext; }

        [JsonIgnore]
        public HttpRequest HttpRequest { get => HttpContext.Request; }

        [JsonIgnore]
        public string UserID { get => HttpContext.User.FindFirst(o => o.Type == ClaimTypes.NameIdentifier).Value; }

        public Tenant CurrentTenant { get => HttpContext.Items["Tenant"] as Tenant; }
        public string ServerBaseUrl { get => HttpContext.GetServerBaseUrl().EnsureTrailingSlash(); }
        public string Origin { get => HttpRequest.Headers["Origin"].ToString(); }
        public string Referer { get => HttpRequest.Headers["Referer"].ToString(); }

        public string NavUrl { get => Referer.TrimStart(Origin + "/").Split("?")[0]; }

    }
}
