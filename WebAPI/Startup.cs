using Autofac;
using Autofac.Extras.DynamicProxy;
using Kernel.Buildin.Dapper;
using Kernel.Buildin.Service;
using Kernel.Core.Extensions;
using Kernel.Core.Multitenant;
using Kernel.Core.Utils;
using Kernel.EF.Demo;
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
            //���⻧
            services.AddBuildinMultitenancy(Configuration);

            //���ý����ļ����ȵ����ֵ
            services.AddBuildinFormOptions();

            //�Զ�����Ȩ����
            services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

            //���jwt��֤��
            services.AddBuildinAuth(Configuration, AuthOptions.AuthConfigure);

            //ע��Dapper���ݿ�����
            services.RegisterDapperConnection(Configuration);

            //�н�ģʽ���
            services.AddMediatR(typeof(Kernel.MediatR.Demo.User.V1_0.GetUserCommandHandler).Assembly);

            //ע�����
            services.RegisterServices();

            //����MvcOptions
            services.AddBuildinMvcOptions();

            //ע�� Microsoft.AspNetCore.Http.IHttpContextAccessor
            services.AddHttpContextAccessor();

            //���AspNetCoreRateLimit
            services.AddBuildinRateLimit(Configuration);

            //������֤
            services.AddBuildinApiBehaviorOptions();

            //���swagger
            services.AddBuildinSwagger();

            //ע��EF
            services.AddDbContext<ReportServerContext>(option => option.UseSqlServer(Configuration.GetSection("DBConnction:SqlServerConnection").Value));

            //Dapper�ֶ�ӳ��
            ColumnMapper.SetMapper();

            //�������
            services.AddBuildinCrossDomain();

            //ʵʱͨѶ
            services.AddSignalR();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //����κ�Autofacģ���ע�ᡣ
            //������ConfigureServices֮����õģ�����
            //�ڴ˴�ע�Ὣ������ConfigureServices��ע������ݡ�
            //�ڹ�������ʱ������á�UseServiceProviderFactory��new AutofacServiceProviderFactory��������`���򽫲�����ôˡ�

            builder.RegisterModule(new AutofacModuleRegister(Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath, new List<string>()
                   { //�������캯��ע��
                        "Kernel.Repository.dll",
                        "Kernel.Service.dll",
                   }));

            //�����Ҫ��Controller��ʹ������ע�룬��Ҫ��ConfigureContainer��������´���
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired()
                .EnableClassInterceptors(); // ������Controller����ʹ��������


            //builder.RegisterType<Log4netService>()
            //       .As<ILogService>()
            //       .PropertiesAutowired()//��ʼ����ע��
            //       .InstancePerLifetimeScope();//��Ϊÿһ����������ô���һ����һ�Ĺ����ʵ��

            //builder.RegisterType<JwtService>()
            //       .As<ITokenService>()
            //       .PropertiesAutowired()//��ʼ����ע��
            //       .InstancePerLifetimeScope();//��Ϊÿһ����������ô���һ����һ�Ĺ����ʵ��

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReportServerContext reportServerContext, IApiVersionDescriptionProvider provider)
        {
            //���� �м����������Ϊ�ڶ� UseRouting �� UseEndpoints�ĵ���֮��ִ�С� ���ò���ȷ�������м��ֹͣ�������У����Ƿŵ���ʼ����Ҳ���ԡ�
            app.AddBuildinCrossDomain();

            app.AddRateLimit();
            app.UseAuthentication();

            ServiceHost.Init(app.ApplicationServices);

            app.UseMultiTenant();

            app.AddBuildinStaticFiles();

            LogHelper.Configure();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //UseSignalR�ѹ�ʱ
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<ChatHub>("/chatHub");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MsgHub>("/msgHub");
                endpoints.MapControllers();
            });

            //reportServerContext.Database.EnsureCreated();//���ݿⲻ���ڵĻ������Զ�����

            app.UseBuiltinRuntime();

            app.AddBuildinSwagger(provider);

        }

    }
}
