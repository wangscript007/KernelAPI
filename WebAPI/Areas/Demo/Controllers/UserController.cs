using Kernel.Core.Basic;
using Kernel.EF.Demo;
using Kernel.IService.Service.Demo;
using Kernel.MediatR.Demo.HelloWorld.V1_0;
using Kernel.Model.Demo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<IActionResult> GetUser_V1_0(string id)
        {
            // http://localhost:39274/v1/demo/user/info/566C32F61CC1204EE0540018FE2DA12B

            //获取API版本号
            var apiVersion = HttpContext.GetRequestedApiVersion().ToString();

            _logger.LogWarning("apiVersion:" + apiVersion);

            SysUserInParams model = new SysUserInParams { userID = id };
            var result = await _mediator.Send(new Kernel.MediatR.Demo.User.V1_0.GetUserCommand(model));
            result.apiVersion = HttpContext.GetRequestedApiVersion().ToString();//获取API版本号
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
            // http://localhost:39274/v2/user/info/566C32F61CC1204EE0540018FE2DA12B

            SysUserInParams model = new SysUserInParams { userID = id };
            var result = await _mediator.Send(new Kernel.MediatR.Demo.User.V2_0.GetUserCommand(model));
            result.apiVersion = HttpContext.GetRequestedApiVersion().ToString();//获取API版本号
            return Ok(result);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetUserList_V1_0([FromQuery]SysUserInParams model)
        {
            //测试 MediatorR 的 Publish 模式
            //await _mediator.Publish(new Pro.Application.Commands.HelloWorld.V1_0.HelloWorldCommand());

            // http://localhost:39274/v1/user/list?pageIndex=1&pageSize=10

            var result = await _mediator.Send(new Kernel.MediatR.Demo.User.V1_0.GetUserListCommand(model));
            result.apiVersion = HttpContext.GetRequestedApiVersion().ToString();//获取API版本号
            return Ok(result);
        }

        [HttpGet]
        [Route("list"), MapToApiVersion("2.0")]
        public async Task<IActionResult> GetUserList_V2_1([FromQuery] SysUserInParams model)
        {
            //测试 MediatorR 的 Publish 模式
            await _mediator.Publish(new HelloWorldCommand());

            ReportServerContext context = new ReportServerContext();
            var users = context.Users;

            return Ok(users.AsQueryable());
        }

        ///// <summary>
        ///// 删除用户信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //[Route("v1/[controller]/{id}")]
        //public async Task<IActionResult> DelUser(string id)
        //{
        //    JsonResult result = new JsonResult(new UserModel { Id = "x0001", UserName = "用户姓名" });
        //    return  result;
        //}

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
