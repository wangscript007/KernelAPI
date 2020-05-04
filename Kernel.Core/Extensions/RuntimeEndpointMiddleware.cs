using Kernel.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Core.Extensions
{
    public class RuntimeEndpointMiddleware
    {
        private readonly RequestDelegate next;
        private readonly JwtSettings jwtSettings;

        public RuntimeEndpointMiddleware(RequestDelegate next, JwtSettings jwtSettings)
        {
            this.next = next;
            this.jwtSettings = jwtSettings;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(KernelApp.Settings, Formatting.None), Encoding.UTF8);
        }
    }
}
