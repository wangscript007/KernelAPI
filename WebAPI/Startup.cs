using Autofac;
using Autofac.Extras.DynamicProxy;
using Kernel.Core.AOP;
using Kernel.Core.Extensions;
using Kernel.Core.Multitenant;
using Kernel.Core.Utils;
using Kernel.EF.Demo;
using Kernel.Model.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WebAPI.Extensions;
using WebAPI.Extensions.AuthHandler;
using WebAPI.Settings;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //跨域配置
        readonly string AllowSpecificOrigins = "_AllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMultitenancy()
                //.WithTenantResolver<HeaderTenantResolver>()
                //.WithTenantStore<JsonFileTenantStore>()
                .WithTenantService(Configuration);

            //设置接收文件长度的最大值。
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

            //添加jwt验证：
            services.AddAuth(Configuration);            

            //注册Dapper数据库连接
            services.RegisterDapperConnection(Configuration);

            services.AddMediatR(typeof(Kernel.MediatR.Demo.User.V1_0.GetUserCommandHandler).Assembly);

            //注册服务
            services.RegisterServices();

            services.AddControllers(config =>
            {
                config.Filters.Add(typeof(GlobalExceptions));
                //config.Filters.Add<AuthFilter>(); //暂时弃用
                config.Filters.Add<ApiLogAttribute>();
            }).AddControllersAsServices()//默认情况下，Controller的参数会由容器创建，但Controller的创建是有AspNetCore框架实现的。要通过容器创建Controller，需要在Startup中配置一下
              .AddNewtonsoftJson();

            //注册 Microsoft.AspNetCore.Http.IHttpContextAccessor
            services.AddHttpContextAccessor();

            //添加AspNetCoreRateLimit
            services.AddRateLimit(Configuration);

            //参数验证
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.GetValidationSummary();
                    var result = new CommandResult<List<string>>()
                    {
                        Success = false,
                        Message = "参数验证不通过",
                        Data = errors
                    };

                    return new JsonResult(result) { StatusCode = StatusCodes.Status416RangeNotSatisfiable };
                };
            });

            // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                    //使用域描述         
                    options.TagActionsBy(apiDesc => apiDesc.CustomTagsSelector());

                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    options.IncludeXmlComments(XmlCommentsFilePath);
                });

            //注册EF
            services.AddDbContext<ReportServerContext>(option => option.UseSqlServer(Configuration.GetSection("DBConnction:SqlServerConnection").Value));

            //Dapper字段映射
            ColumnMapper.SetMapper();

            //允许跨域
            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificOrigins,
                builder => builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());
            });

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
            //跨域 中间件必须配置为在对 UseRouting 和 UseEndpoints的调用之间执行。 配置不正确将导致中间件停止正常运行，但是放到开始好像也可以。
            app.UseCors(AllowSpecificOrigins);

            app.AddRateLimit();
            app.UseAuthentication();

            ServiceHost.Init(app.ApplicationServices);

            app.UseMultiTenant();

            app.UseStaticFiles(new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
                FileProvider = new PhysicalFileProvider
                (
                    //本地资源路径
                    //注：在linux下，这个路径不能以斜杠结尾，不然会报错：Request path must not end in a slash
                    KernelApp.Settings.ResourcesRootPath
                ),
                //URL路径,URL路径可以自定义，可以不用跟本地资源路径一致
                RequestPath = new PathString("/" + KernelApp.Settings.ResourcesRootFolder)
            });

            LogHelper.Configure();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //reportServerContext.Database.EnsureCreated();//数据库不存在的话，会自动创建

            app.UseBuiltinRuntime();

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(KernelApp.Settings.BasePath, fileName);
            }
        }
    }
}
