using Kernel.Core.AOP;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class MvcOptionsExtensions
    {
        public static void AddBuildinMvcOptions(this IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.Filters.Add(typeof(GlobalExceptions));
                //config.Filters.Add<AuthFilter>(); //暂时弃用
                config.Filters.Add<ApiLogAttribute>();
            }).AddControllersAsServices()//默认情况下，Controller的参数会由容器创建，但Controller的创建是有AspNetCore框架实现的。要通过容器创建Controller，需要在Startup中配置一下
              .AddNewtonsoftJson();

        }

    }
}
