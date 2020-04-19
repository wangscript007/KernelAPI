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
            result.errCode = OverallErrCode.ERR_VER_PAR;
            result.success = true;

            try
            {
                var headrs = context.HttpContext.Request.Headers;
                string token = headrs["Authorization"];
                if (token == null)
                {
                    result.success = false;
                    result.message = "请求无效，未识别到token!";
                    result.errCode = OverallErrCode.ERR_VER_TOKEN_NOTFOUND;
                    result.resCode = OverallResCode.PARAM_IS_INVALID;
                    context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };
                    return;
                }

                var claims = JwtUtil.DecodeToken(token);

                var userName = claims.FirstOrDefault(m => m.Type == "UserName").Value;
                var validTime = DateTime.Parse(claims.FirstOrDefault(m => m.Type == "ValidTime").Value);

                if (validTime < DateTime.Now)
                {
                    result.success = false;
                    result.message = "token已过期，请重新登录!";
                    result.errCode = OverallErrCode.ERR_VER_TOKEN_EXPIRED;
                    result.resCode = OverallResCode.PARAM_IS_INVALID;
                    context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };
                    return;
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = "无效token!";
                result.errCode = OverallErrCode.ERR_VER_TOKEN_INVALID;
                result.resCode = OverallResCode.PARAM_IS_INVALID;
                result.data = ex.Message;
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
