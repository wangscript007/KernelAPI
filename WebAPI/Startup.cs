using Autofac;
using Autofac.Extras.DynamicProxy;
using Kernel.Core;
using Kernel.Core.AOP;
using Kernel.Core.Extensions;
using Kernel.Core.Models;
using Kernel.Core.Multitenant;
using Kernel.Core.Utils;
using Kernel.EF.Demo;
using Kernel.Model.Core;
using Kernel.Repository.Core;
using Kernel.Service.Core;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using WebAPI.Extensions;
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

        //��������
        readonly string AllowSpecificOrigins = "_AllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var jwtSettings = new JwtSettings();
            Configuration.GetSection("JwtSettings").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddMultitenancy()
                //.WithTenantResolver<HeaderTenantResolver>()
                //.WithTenantStore<JsonFileTenantStore>()
                .WithTenantService(Configuration);

            //���ý����ļ����ȵ����ֵ��
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            //���jwt��֤��
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //Token�䷢����
                    ValidIssuer = jwtSettings.Issuer,
                    //�䷢��˭
                    ValidAudience = jwtSettings.Audience,
                    //�����keyҪ���м��ܣ���Ҫ����Microsoft.IdentityModel.Tokens
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    //ValidateIssuerSigningKey=true,
                    ////�Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                    ValidateLifetime = true,
                    ////����ķ�����ʱ��ƫ����
                    //ClockSkew=TimeSpan.Zero
                };
            });

            //ע��Dapper���ݿ�����
            services.RegisterDapperConnection(Configuration);

            services.AddMediatR(typeof(Kernel.MediatR.Demo.User.V1_0.GetUserCommandHandler).Assembly);

            //ע�����
            services.RegisterServices();

            services.AddControllers(config =>
            {
                config.Filters.Add(typeof(GlobalExceptions));
                //config.Filters.Add<AuthFilter>(); //��ʱ����
            }).AddControllersAsServices()//Ĭ������£�Controller�Ĳ�������������������Controller�Ĵ�������AspNetCore���ʵ�ֵġ�Ҫͨ����������Controller����Ҫ��Startup������һ��
              .AddNewtonsoftJson();

            //ע�� Microsoft.AspNetCore.Http.IHttpContextAccessor
            services.AddHttpContextAccessor();

            //������֤
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.GetValidationSummary();
                    var result = new CommandResult<List<string>>()
                    {
                        Success = false,
                        Message = "������֤��ͨ��",
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
                    //ʹ��������         
                    options.TagActionsBy(apiDesc => apiDesc.CustomTagsSelector());

                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    options.IncludeXmlComments(XmlCommentsFilePath);
                });

            //ע��EF
            services.AddDbContext<ReportServerContext>(option => option.UseSqlServer(Configuration.GetSection("DBConnction:SqlServerConnection").Value));

            //Dapper�ֶ�ӳ��
            ColumnMapper.SetMapper();

            //�������
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
            app.UseAuthentication();

            ServiceHost.Init(app.ApplicationServices);

            app.UseMultiTenant();

            app.UseStaticFiles(new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
                FileProvider = new PhysicalFileProvider
                (
                    //������Դ·��
                    KernelApp.Settings.ResourcesRootPath
                ),
                //URL·��,URL·�������Զ��壬���Բ��ø�������Դ·��һ��
                RequestPath = new PathString("/" + KernelApp.Settings.ResourcesRootFolder)
            });

            LogHelper.Configure();

            if (env.IsDevelopment())
            {
                KernelApp.Settings.IsDevelopment = true;
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //���� �м����������Ϊ�ڶ� UseRouting �� UseEndpoints�ĵ���֮��ִ�С� ���ò���ȷ�������м��ֹͣ�������С�
            app.UseCors(AllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //reportServerContext.Database.EnsureCreated();//���ݿⲻ���ڵĻ������Զ�����

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
