using Kernel.Core.Utils;
using Microsoft.AspNetCore.Builder;
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

        public static void AddBuildinLogConfigure(this IApplicationBuilder app)
        {
            LogHelper.Configure();

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


    }
}
