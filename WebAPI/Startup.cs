using Autofac;
using Kernel.Buildin.Service;
using Kernel.Core.Extensions;
using Kernel.Model.Demo;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAPI.Configure;

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
            //填充配置
            services.AddBuildinOption(Configuration);

            //注册服务
            services.RegisterServices(Configuration);

            //多租户
            services.AddBuildinMultitenancy(Configuration);

            //设置接收文件长度的最大值
            services.AddBuildinFormOptions();

            //添加jwt验证：
            services.AddBuildinAuth(Configuration, AuthOptions.AuthConfigure);

            //注册Dapper数据库连接
            services.RegisterDapperConnection(Configuration);

            //中介模式组件
            services.AddMediatR(typeof(Kernel.MediatR.Demo.User.V1_0.GetUserCommandHandler).Assembly);

            //配置MvcOptions
            services.AddBuildinMvcOptions();

            //添加AspNetCoreRateLimit
            services.AddBuildinRateLimit(Configuration);

            //参数验证
            services.AddBuildinApiBehaviorOptions();

            //添加swagger
            services.AddBuildinSwagger();

            //注册EF
            services.AddBuildinEntityFramework(Configuration);

            //Dapper字段映射
            services.AddBuildinDapperMapper(typeof(SysUser));

            //允许跨域
            services.AddBuildinCrossDomain();

            //实时通讯
            services.AddSignalR();

            //RabbitMQ发布和订阅
            services.AddBuildinRabbitMQ(Configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddBuildinAutofac();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //网关转发时，填充协议头remoteIp、scheme
            app.AddBuildinForwardedHeaders();

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

            //EF配置
            app.AddBuildinEntityFramework();

            //well-known/runtime
            app.UseBuiltinRuntime();

            //配置swagger
            app.AddBuildinSwagger();

            //注册consul
            Configuration.ConsulRegister();

        }

    }
}
