using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.Extensions
{
    public static class RuntimeBuilderExtensions
    {
        public static IApplicationBuilder UseBuiltinRuntime(this IApplicationBuilder app)
        {
            app.UseWhen(
                context =>
                {
                    return context.Request.Path.Value.ToLower() == "/well-known/runtime";
                },
                appBuilder =>
                {
                    appBuilder.UseMiddleware<RuntimeEndpointMiddleware>();
                }
            );

            return app;
        }
    }
}
