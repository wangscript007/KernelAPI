using Kernel.Buildin.Swagger;
using Kernel.Core.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class SwaggerExtensions
    {
        public static void AddBuildinSwagger(this IServiceCollection services)
        {
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

                    #region 加锁
                    var openApiSecurity = new OpenApiSecurityScheme
                    {
                        Description = "JWT认证授权，使用直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                        Name = "Authorization",  //jwt 默认参数名称
                        In = ParameterLocation.Header,  //jwt默认存放Authorization信息的位置（请求头）
                        Type = SecuritySchemeType.ApiKey
                    };

                    options.AddSecurityDefinition("oauth2", openApiSecurity);
                    options.OperationFilter<AddResponseHeadersFilter>();
                    options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                    #endregion
                });

            //让JsonProperty属性生效
            services.AddSwaggerGenNewtonsoftSupport();
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var fileName = Assembly.GetEntryAssembly().GetName().Name + ".xml";
                return Path.Combine(KernelApp.Settings.BasePath, fileName);
            }
        }

        public static void AddBuildinSwagger(this IApplicationBuilder app)
        {
            IApiVersionDescriptionProvider provider = ServiceHost.GetService<IApiVersionDescriptionProvider>();
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
    }
}
