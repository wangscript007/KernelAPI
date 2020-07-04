using Kernel.Core.AOP;
using Kernel.Core.Basic;
using Kernel.Core.Utils;
using Kernel.EF.Demo;
using Kernel.IService.Service.Demo;
using Kernel.MediatR.Demo.HelloWorld.V1_0;
using Kernel.Model.Demo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Areas.Demo.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    public class UserController : DemoBaseController
    {
        private IMediator _mediator;
        ILogger<UserController> _logger;

        public IEmailService _emailService { get; set; }

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("info/{id}"), MapToApiVersion("1.0")]
        //[Authorize(Roles = "admin,role1")] //admin、role1两个角色任意满足一个
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "Permission")] //权限验证策略   注：不同的Authorize之间是并且的关系
        public async Task<IActionResult> GetUser_V1_0(string id)
        {
            // http://localhost:39274/api/v1/demo/user/info/566C32F61CC1204EE0540018FE2DA12B

            //获取API版本号
            var apiVersion = HttpContext.GetRequestedApiVersion().ToString();

            _logger.LogWarning("apiVersion:" + apiVersion);

            SysUserInParams model = new SysUserInParams { userID = id };
            var result = await _mediator.Send(new Kernel.MediatR.Demo.User.V1_0.GetUserCommand(model));
            //result.apiVersion = HttpContext.GetRequestedApiVersion().ToString();//获取API版本号
            return Ok(result);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("info/{id}"), MapToApiVersion("2.0")]
        public async Task<IActionResult> GetUser_V2_0(string id)
        {
            // http://localhost:39274/api/v2/user/info/566C32F61CC1204EE0540018FE2DA12B

            SysUserInParams model = new SysUserInParams { userID = id };
            var result = await _mediator.Send(new Kernel.MediatR.Demo.User.V2_0.GetUserCommand(model));
            //result.apiVersion = HttpContext.GetRequestedApiVersion().ToString();//获取API版本号
            return Ok(result);
        }

        [HttpGet]
        [Route("list"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUserList_V1_0([FromQuery]SysUserInParams model)
        {
            //测试 MediatorR 的 Publish 模式
            //await _mediator.Publish(new Pro.Application.Commands.HelloWorld.V1_0.HelloWorldCommand());

            // http://localhost:39274/api/v1/user/list?pageIndex=1&pageSize=10

            var result = await _mediator.Send(new Kernel.MediatR.Demo.User.V1_0.GetUserListCommand(model));
            //result.apiVersion = HttpContext.GetRequestedApiVersion().ToString();//获取API版本号
            return Ok(result);
        }

        [HttpGet]
        [Route("list"), MapToApiVersion("2.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserList_V2_1([FromQuery] SysUserInParams model)
        {
            //测试 MediatorR 的 Publish 模式
            await _mediator.Publish(new HelloWorldCommand());

            //ReportServerContext context = new ReportServerContext();
            //var users = context.Users;

            //生成token示例
            var claims = new Claim[]{
                new Claim(ClaimTypes.Name,"wyt"),
                new Claim(ClaimTypes.Role,"admin"),
                new Claim(ClaimTypes.Role,"role1"),
                new Claim("Tenant","2c30ceb7534f44869bd0487e187bb34d")
            };
            string token = JwtUtil.EncodeToken(claims);


            //return Ok(users.AsQueryable());
            return Ok(token);
        }

        [HttpGet]
        [Route("WriteCookie/{flag}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> WriteCookie_V1_0(string flag)
        {
            Response.Cookies.Append("cookie_" + flag, flag);

            var result = new {
                RequestCookies = Request.Cookies
            };

            return Ok(result);
        }

        /// <summary>
        /// 演示跨服务http调用传递cookie，存储被调服务cooike再次请求（适用于登录后爬虫抓数据的场景）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ReadCookie"), MapToApiVersion("1.0")]
        public async Task<IActionResult> ReadCookie_V1_0()
        {
            //初始化容器
            CookieContainer cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookieContainer;

            //写个cookie
            Cookie cookie = new Cookie();
            cookie.Name = "cookie_0";
            cookie.Value = "0";
            cookie.Domain = "localhost";
            handler.CookieContainer.Add(cookie);

            var uri = new Uri("http://localhost:39274");

            //初始化httpClient
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = uri,
                Timeout = TimeSpan.FromSeconds(3), // Very slow to respond, this server                
            };

            //第一次请求
            ISomeApi api = RestClient.For<ISomeApi>(httpClient);
            var result = await api.WriteCookie_V1_0("1");

            //第二次携带cookie请求
            ISomeApi api2 = RestClient.For<ISomeApi>(httpClient);
            var result2 = await api2.WriteCookie_V1_0("2");

            StringBuilder sb_cookie = new StringBuilder();
            List<Cookie> cookies = cookieContainer.GetCookies(uri).Cast<Cookie>().ToList();

            return Ok(cookies);
        }

        [BasePath("api/v1/Demo/User")]
        public interface ISomeApi
        {
            [Get("WriteCookie/{flag}")]
            Task<string> WriteCookie_V1_0([Path("flag")] string flag);
        }

        ///// <summary>
        ///// 获取指定用户姓名
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("v1/[controller]/{id}/{type}")]
        //public IActionResult GetUserName(string id,string type)
        //{
        //    JsonResult result = new JsonResult(new { UserName = "用户姓名" });
        //    return result;
        //}

        //[HttpPost]
        //[Route("v1/[controller]")]
        //[ProducesResponseType(typeof(String), (int)HttpStatusCode.BadRequest)]
        //public IActionResult CreateUser([FromBody] UserModel user)
        //{
        //    try
        //    {
        //        //throw new Exception("手动异常");
        //        JsonResult result = new JsonResult( user );
        //        return result;
        //        //return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("这里是异常提示"+ex.Message);
        //    }   
        //}


    }
}
