using Kernel.Core.Models;
using Kernel.Core.Utils;
using Kernel.Dapper.Factory;
using Kernel.IService.Repository.System;
using Kernel.Model.System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Extensions.AuthHandler;
using Kernel.Core.Extensions;

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

        public static void AddAuth(this IServiceCollection services, IConfiguration Configuration)
        {
            var jwtSettings = new JwtSettings();
            Configuration.GetSection("JwtSettings").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthorization(options =>
            {
                //添加授权策略
                options.AddPolicy("ApiPerm",
                   policy =>
                   {
                       //policy.RequireClaim(ClaimTypes.Role, "admin", "role1");
                       //policy.RequireUserName("wyt2");
                       policy.RequireAssertion((context) =>
                       {
                           //token过期或不正确时，是解析不到UserID的
                           var userID = KernelApp.Request.UserID;
                           if (userID == null)
                               return false;

                           var navUrl = KernelApp.Request.NavUrl;
                           var resPath = (context.Resource as RouteEndpoint).DisplayName;
                           ISysFuncPermRepository permRep = ServiceHost.GetScopeService<ISysFuncPermRepository>();
                           bool result = permRep.HasApiPerm_V1_0(resPath).Result;
                           if(!result)
                           {
                               LogHelper.log.Info("接口验权失败！");
                               LogHelper.log.Info(userID);
                               LogHelper.log.Info(navUrl);
                               LogHelper.log.Info(resPath);
                           }
                           return result;

                           //return context.User.Claims.Any(o => o.Type == ClaimTypes.Name && o.Value == "wyt");
                       });
                   }
                );
                //自定义授权策略
                options.AddPolicy("AtLeast21", policy =>
                    policy.Requirements.Add(new MinimumAgeRequirement(21)));

            })
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                //自定义验证token
                //options.SecurityTokenValidators.Clear();//原先默认的验证方法清除
                //options.SecurityTokenValidators.Add(new MyTokenValidator());

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //Token颁发机构
                    ValidIssuer = jwtSettings.Issuer,
                    //颁发给谁
                    ValidAudience = jwtSettings.Audience,
                    //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    //ValidateIssuerSigningKey=true,
                    ////是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    ValidateLifetime = true,
                    ////允许的服务器时间偏移量
                    //ClockSkew=TimeSpan.Zero
                };
                options.SaveToken = true;
                options.Events = new JwtBearerEvents()
                {
                    // 在安全令牌通过验证和ClaimsIdentity通过验证之后调用
                    // 如果用户访问注销页面
                    OnTokenValidated = context =>
                                {
                                    if (context.Request.Path.Value.ToString() == "/account/logout")
                                    {
                                        var token = ((context as TokenValidatedContext).SecurityToken as JwtSecurityToken).RawData;
                                    }
                                    return Task.CompletedTask;
                                },
                    //自定义jwttoken时会用到OnMessageReceived事件，从Headers里面获取token放到MessageReceivedContext
                    //OnMessageReceived = context =>
                    //{
                    //    var token = context.Request.Headers["Authorization"];
                    //    context.Token = token.FirstOrDefault();
                    //    return Task.CompletedTask;
                    //}
                };
            });
        }

    }

}