using Autofac;
using Autofac.Extras.DynamicProxy;
using Kernel.Core.AOP;
using Kernel.Core.Utils;
using Kernel.EF.Demo;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.IO;
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
        readonly string AllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Secret"]))//�õ�SecurityKey
                };
            });

            //ע��Dapper���ݿ�����
            services.RegisterDapperConnection(Configuration);

            services.AddMediatR(typeof(Kernel.MediatR.Demo.User.V1_0.GetUserCommandHandler).Assembly);

            //ע�����
            services.RegisterServices();

            services.AddControllers(config => config.Filters.Add<AuthFilter>())
                .AddControllersAsServices()//Ĭ������£�Controller�Ĳ�������������������Controller�Ĵ�������AspNetCore���ʵ�ֵġ�Ҫͨ����������Controller����Ҫ��Startup������һ��
                .AddNewtonsoftJson();

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
            ServiceHost.Load(app.ApplicationServices);

            if (env.IsDevelopment())
            {
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

            reportServerContext.Database.EnsureCreated();//���ݿⲻ���ڵĻ������Զ�����

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
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
