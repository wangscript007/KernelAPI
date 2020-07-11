using Kernel.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class AuthExtensions
    {
        public static void AddBuildinAuth(this IServiceCollection services, IConfiguration Configuration, Action<AuthorizationOptions> configure)
        {
            var jwtSettings = new JwtSettings();
            Configuration.GetSection("JwtSettings").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthorization(configure)
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
                options.SaveToken = true;//默认为true，可以通过HttpContext.GetTokenAsync("Bearer", "access_token").Result获取当前请求的token
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


                    OnMessageReceived = (context) => {
                        if (!context.HttpContext.Request.Path.HasValue)
                        {
                            return Task.CompletedTask;
                        }
                        //重点在于这里；判断是Signalr的路径
                        var accessToken = context.HttpContext.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrWhiteSpace(accessToken))
                        {
                            context.Token = accessToken;
                            return Task.CompletedTask;
                        }
                        return Task.CompletedTask;
                    }

                };
            });

        }

    }
}
