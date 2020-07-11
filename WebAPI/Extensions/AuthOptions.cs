using Kernel.Core.Utils;
using Kernel.IService.Repository.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Extensions.AuthHandler;

namespace WebAPI.Extensions
{
    public class AuthOptions
    {
        public static void AuthConfigure(AuthorizationOptions options)
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
                       if (!result)
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
        }
    }
}
