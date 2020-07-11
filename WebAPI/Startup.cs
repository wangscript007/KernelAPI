using Autofac;
using Autofac.Extras.DynamicProxy;
using Kernel.Buildin.Service;
using Kernel.Core.Extensions;
using Kernel.Core.Utils;
using Kernel.EF.Demo;
using Kernel.Model.Demo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Extensions;
using WebAPI.Extensions.AuthHandler;

namespace WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //多租户
            services.AddBuildinMultitenancy(Configuration);

            //设置接收文件长度的最大值
            services.AddBuildinFormOptions();

            //自定义授权策略
            services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

            //添加jwt验证：
            services.AddBuildinAuth(Configuration, AuthOptions.AuthConfigure);

            //注册Dapper数据库连接
            services.RegisterDapperConnection(Configuration);

            //中介模式组件
            services.AddMediatR(typeof(Kernel.MediatR.Demo.User.V1_0.GetUserCommandHandler).Assembly);

            //注册服务
            services.RegisterServices();

            //配置MvcOptions
            services.AddBuildinMvcOptions();

            //注册 Microsoft.AspNetCore.Http.IHttpContextAccessor
            services.AddHttpContextAccessor();

            //添加AspNetCoreRateLimit
            services.AddBuildinRateLimit(Configuration);

            //参数验证
            services.AddBuildinApiBehaviorOptions();

            //添加swagger
            services.AddBuildinSwagger();

            //注册EF
            services.AddDbContext<ReportServerContext>(option => option.UseSqlServer(Configuration.GetSection("DBConnction:SqlServerConnection").Value));

            //Dapper字段映射
            services.AddBuildinDapperMapper(typeof(SysUser));

            //允许跨域
            services.AddBuildinCrossDomain();

            //实时通讯
            services.AddSignalR();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //添加任何Autofac模块或注册。
            //这是在ConfigureServices之后调用的，所以
            //在此处注册将覆盖在ConfigureServices中注册的内容。
            //在构建主机时必须调用“UseServiceProviderFactory（new AutofacServiceProviderFactory（））”`否则将不会调用此。

            builder.RegisterModule(new AutofacModuleRegister(Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath, new List<string>()
                   { //批量构造函数注入
                        "Kernel.Repository.dll",
                        "Kernel.Service.dll",
                   }));

            //如果需要在Controller中使用属性注入，需要在ConfigureContainer中添加如下代码
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired()
                .EnableClassInterceptors(); // 允许在Controller类上使用拦截器


            //builder.RegisterType<Log4netService>()
            //       .As<ILogService>()
            //       .PropertiesAutowired()//开始属性注入
            //       .InstancePerLifetimeScope();//即为每一个依赖或调用创建一个单一的共享的实例

            //builder.RegisterType<JwtService>()
            //       .As<ITokenService>()
            //       .PropertiesAutowired()//开始属性注入
            //       .InstancePerLifetimeScope();//即为每一个依赖或调用创建一个单一的共享的实例

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReportServerContext reportServerContext, IApiVersionDescriptionProvider provider)
        {
            //跨域
            app.AddBuildinCrossDomain();

            //请求频率控制
            app.AddBuildinRateLimit();

            //验权
            app.UseAuthentication();

            //初始化ServiceHost
            app.AddBuildinServiceHost();

            //多租户
            app.AddBuildinMultitenancy();

            //静态资源
            app.AddBuildinStaticFiles();

            //日志配置
            app.AddBuildinLogConfigure();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //授权
            app.UseAuthorization();

            //SignalR Hub
            app.AddBuildinMsgHub<MsgHub>("/msgHub");

            //reportServerContext.Database.EnsureCreated();//数据库不存在的话，会自动创建

            //well-known/runtime
            app.UseBuiltinRuntime();

            //配置swagger
            app.AddBuildinSwagger(provider);

        }

    }
}
