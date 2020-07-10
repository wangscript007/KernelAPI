using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class CrossDomainExtensions
    {
        //跨域配置
        readonly static string AllowSpecificOrigins = "_AllowSpecificOrigins";

        public static void AddBuildinCrossDomain(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificOrigins,
                builder => builder
                .SetIsOriginAllowed(origin => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
            });

        }

        public static void AddBuildinCrossDomain(this IApplicationBuilder app)
        {
            app.UseCors(AllowSpecificOrigins);

        }

    }
}
