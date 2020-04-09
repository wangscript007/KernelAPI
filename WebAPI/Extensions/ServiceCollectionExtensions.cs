using Kernel.Dapper.Factory;
using Kernel.IService.Repository.Demo;
using Kernel.Model.Core;
using Kernel.Repository.Demo;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var DapperFactory = provider.GetService<IDapperFactory>();

            //注册Repository
            //services.AddScoped<IUserRepository>((iServiceProvider) => DapperFactory.CreateRepository<UserRepository>(DapperConst.QYPT_ORACLE));

        }
    }
}