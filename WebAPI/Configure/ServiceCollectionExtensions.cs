using Kernel.Dapper.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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

        }

    }

}