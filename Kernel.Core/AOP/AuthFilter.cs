using Kernel.Core.Extensions;
using Kernel.Core.Models;
using Kernel.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Kernel.Core.AOP
{
    /// <summary>
    /// 安全认证拦截
    /// 被拦截请求的Headers中需要带上Authorization参数(即token)
    /// </summary>
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 判断是否检查登陆
            var noNeedCheck = false;
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                //判断controller是否带NoSign标记
                noNeedCheck = controllerActionDescriptor.FilterDescriptors.Any(o => o.Filter.GetType() == typeof(NoAuthAttribute));

                //判断action是否带NoSign标记
                noNeedCheck = noNeedCheck || controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(NoAuthAttribute)));
            }
            if (noNeedCheck) return;

            HttpRequest request = context.HttpContext.Request;

            //var method = request.Method.ToLower();            
            //string controller = context.RouteData.Values["Controller"].ToString();           
            //string action = context.RouteData.Values["Action"].ToString();

            OverallResult<string> result = new OverallResult<string>();
            result.Success = true;

            try
            {
                var headrs = context.HttpContext.Request.Headers;
                string token = headrs["Authorization"];
                if (token == null)
                {
                    result.Success = false;
                    result.Message = "请求无效，未识别到token!";
                    context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };
                    return;
                }

                token = token.TrimStart("Bearer", true);

                var claims = JwtUtil.DecodeToken(token);

                var userName = claims.FirstOrDefault(m => m.Type == "UserName").Value;
                var validTime = DateTime.Parse(claims.FirstOrDefault(m => m.Type == "ValidTime").Value);

                if (validTime < DateTime.Now)
                {
                    result.Success = false;
                    result.Message = "token已过期，请重新登录!";
                    context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };
                    return;
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "无效token!";
                result.Data = ex.Message;
                context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
        }

    }

    /// <summary>
    /// 不需要token验证的地方加个特性
    /// </summary>
    public class NoAuthAttribute : ActionFilterAttribute { }
}
