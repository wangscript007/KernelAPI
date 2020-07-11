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
            //ע�����
            services.RegisterServices();

            //���⻧
            services.AddBuildinMultitenancy(Configuration);

            //���ý����ļ����ȵ����ֵ
            services.AddBuildinFormOptions();

            //���jwt��֤��
            services.AddBuildinAuth(Configuration, AuthOptions.AuthConfigure);

            //ע��Dapper���ݿ�����
            services.RegisterDapperConnection(Configuration);

            //�н�ģʽ���
            services.AddMediatR(typeof(Kernel.MediatR.Demo.User.V1_0.GetUserCommandHandler).Assembly);

            //����MvcOptions
            services.AddBuildinMvcOptions();

            //���AspNetCoreRateLimit
            services.AddBuildinRateLimit(Configuration);

            //������֤
            services.AddBuildinApiBehaviorOptions();

            //���swagger
            services.AddBuildinSwagger();

            //ע��EF
            services.AddBuildinEntityFramework(Configuration);

            //Dapper�ֶ�ӳ��
            services.AddBuildinDapperMapper(typeof(SysUser));

            //�������
            services.AddBuildinCrossDomain();

            //ʵʱͨѶ
            services.AddSignalR();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddBuildinAutofac();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //����
            app.AddBuildinCrossDomain();

            //����Ƶ�ʿ���
            app.AddBuildinRateLimit();

            //��Ȩ
            app.UseAuthentication();

            //��ʼ��ServiceHost
            app.AddBuildinServiceHost();

            //���⻧
            app.AddBuildinMultitenancy();

            //��̬��Դ
            app.AddBuildinStaticFiles();

            //��־����
            app.AddBuildinLogConfigure();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //��Ȩ
            app.UseAuthorization();

            //SignalR Hub
            app.AddBuildinMsgHub<MsgHub>("/msgHub");

            //EF����
            app.AddBuildinEntityFramework();

            //well-known/runtime
            app.UseBuiltinRuntime();

            //����swagger
            app.AddBuildinSwagger();

        }

    }
}
