using FreeSql;
using Kernel.IService.Repository.Demo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebAPI.Configure.AuthHandler;

namespace WebAPI.Configure
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            //var provider = services.BuildServiceProvider();
            //var DapperFactory = provider.GetService<IDapperFactory>();

            //注册Repository
            //services.AddScoped<IUserRepository>((iServiceProvider) => DapperFactory.CreateRepository<UserRepository>(DapperConst.QYPT_ORACLE));

            //自定义授权策略
            services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

            var fsql1 = new FreeSqlBuilder().UseConnectionString(DataType.MySql, configuration.GetSection("DBConnction:MySQLConnection").Value)
                .UseMonitorCommand(cmd =>//执行前
                {
                    Console.WriteLine("--------------------------------------------------执行前begin--------------------------------------------------");
                    Console.WriteLine(cmd.CommandText);
                    Console.WriteLine("--------------------------------------------------执行前end--------------------------------------------------");
                }, (cmd, valueString) =>//执行后
                {
                    Console.WriteLine("--------------------------------------------------执行后begin--------------------------------------------------");
                    Console.WriteLine(cmd.CommandText);
                    Console.WriteLine(valueString);
                    Console.WriteLine("--------------------------------------------------执行后end--------------------------------------------------");
                })
                .UseAutoSyncStructure(true).Build<MySqlFlag>();

            services.AddSingleton<IFreeSql<MySqlFlag>>(fsql1);
        }

    }

}