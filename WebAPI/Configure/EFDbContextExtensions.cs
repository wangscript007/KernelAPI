using Kernel.Core.Utils;
using Kernel.EF.Demo;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Configure
{
    public static class EFDbContextExtensions
    {
        public static void AddBuildinEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReportServerContext>(option => option.UseSqlServer(configuration.GetSection("DBConnction:SqlServerConnection").Value));
        }

        public static void AddBuildinEntityFramework(this IApplicationBuilder app)
        {
            //var reportServerContext = ServiceHost.GetService<ReportServerContext>();
            //reportServerContext.Database.EnsureCreated();//数据库不存在的话，会自动创建
        }

    }
}
