using Kernel.Core.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class ApplicationBuilderExtensions
    {
        public static void AddBuildinServiceHost(this IApplicationBuilder app)
        {
            ServiceHost.Init(app.ApplicationServices);

        }

        public static void AddBuildinMsgHub<T>(this IApplicationBuilder app, string pattern) where T : Hub
        {
            //UseSignalR已过时
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<ChatHub>("/chatHub");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<T>(pattern);
                endpoints.MapControllers();
            });

        }

        public static void AddBuildinForwardedHeaders(this IApplicationBuilder app)
        {
            ForwardedHeadersOptions options = new ForwardedHeadersOptions();
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
            app.UseForwardedHeaders(options);
        }

    }
}
