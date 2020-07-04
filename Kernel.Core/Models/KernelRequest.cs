using Kernel.Core.Extensions;
using Kernel.Core.Multitenant;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string UserID { get => HttpContext.User.FindFirst(o => o.Type == ClaimTypes.NameIdentifier)?.Value; }
        public IEnumerable<string> RoleIDs { get => HttpContext.User.Claims.Where(o => o.Type == ClaimTypes.Role).Select(o => o.Value); }

        public Tenant CurrentTenant { get => HttpContext.Items["Tenant"] as Tenant; }
        public string ServerBaseUrl { get => HttpContext.GetServerBaseUrl().EnsureTrailingSlash(); }
        public string FileBaseUrl { get => $"{ServerBaseUrl}{KernelApp.Settings.ResourcesRootFolder}/{KernelApp.Settings.AttachmentFolder}/"; }
        public string Origin { get => HttpRequest.Headers["Origin"].ToString(); }
        public string Referer { get => HttpRequest.Headers["Referer"].ToString(); }

        public string NavUrl { get => Referer.TrimStart(Origin + "/").Split("?")[0]; }

        public string RemoteIpAddress { get => HttpContext.Connection.RemoteIpAddress.ToString(); }

        public string AccessToken { get => HttpContext.GetTokenAsync("Bearer", "access_token").Result; }

    }
}
