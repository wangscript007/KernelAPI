using Kernel.Core.Utils;
using Kernel.IService.Repository.System;
using Kernel.Model.Core;
using Kernel.Model.System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Areas.System.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(Policy = "ApiPerm")]
    public class SysUserController : SystemBaseController
    {
        private IMediator _mediator;
        public ISysUserRepository SysUserRepository { get; set; }
        public ISysRoleRepository SysRoleRepository { get; set; }

        public SysUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("login"), MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> SysUserLogin_V1_0([FromQuery]SysUser user)
        {
            var result = new CommandResult<SysUserLogin> { Success = false };

            var model = await SysUserRepository.GetSysUserByLoginID_V1_0(user.LoginID);
            if (model == null)
                result.Message = "登录失败，该用户不存在！";
            else if (model.IsLocked == "1")
                result.Message = "登录失败，账号为锁定状态！";
            else if (model.DictIsActive == "0")
                result.Message = "登录失败，账号已作废！";
            else if (SecurityHelper.EncryptDES(user.LoginPwd) != model.LoginPwd)
                result.Message = "登录失败，密码错误！";
            else
            {
                //获取角色信息
                var roles = await SysRoleRepository.GetUserRoles_V1_0(model.UserID);
                if(roles.Count() == 0)
                {
                    result.Message = "登录失败，未配置角色！";
                    return Ok(result);
                }

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, model.UserID));

                foreach (var roleID in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleID));
                }

                model.Token = JwtUtil.EncodeToken(claims);
                result.Data = model;
                result.Success = true;
                result.Message = "登录成功！";
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("SysUser/{userID}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSysUser_V1_0(string userID)
        {
            var user = await SysUserRepository.GetSysUser_V1_0(userID);
            var result = new CommandResult<SysUser> { Data = user };

            return Ok(result);
        }

        [HttpGet]
        [Route("SysUser"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSysUserList_V1_0([FromQuery]SysUserListIn model)
        {
            var result = await SysUserRepository.GetSysUserList_V1_0(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("SysUser"), MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveSysUser_V1_0([FromBody]SysUser model)
        {
            if (string.IsNullOrEmpty(model.UserID))
            {
                model.UserID = KernelApp.Settings.NewGUID;
                model.LoginPwd = SecurityHelper.EncryptDES("1");
                model.CreateBy = KernelApp.Request.UserID;
                model.CreateTime = DateTime.Now;
                model.UpdateBy = KernelApp.Request.UserID;
                model.UpdateTime = DateTime.Now;
                await SysUserRepository.AddSysUser_V1_0(model);
            }
            else
            {
                model.UpdateBy = KernelApp.Request.UserID;
                model.UpdateTime = DateTime.Now;
                await SysUserRepository.UpdateSysUser_V1_0(model);
            }

            var result = new CommandResult<string> { Data = model.UserID };

            return Ok(result);
        }

        [HttpPost]
        [Route("IsActive"), MapToApiVersion("1.0")]
        public async Task<IActionResult> SetSysUserIsActive_V1_0([FromBody]SysUser model)
        {
            var user = await SysUserRepository.GetSysUser_V1_0(model.UserID);
            user.DictIsActive = model.DictIsActive;
            await SysUserRepository.UpdateSysUser_V1_0(user);

            var result = new CommandResult<bool> { Data = true };

            var hub = ServiceHost.GetScopeService<IHubContext<MsgHub>>();
            hub.Clients.User(KernelApp.Request.UserID).SendAsync("ReceiveMessage", new { Event = "MSGHUB_SYSUSER_ISACTIVE_CHAHGED", data = "设置成功！"});

            return Ok(result);
        }

        [HttpDelete]
        [Route("SysUser"), MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteSysUser_V1_0([FromQuery]SysUser model)
        {
            var count = await SysUserRepository.DeleteSysUser_V1_0(model.UserID.Split(','));
            var result = new CommandResult<int> { Data = count };

            return Ok(result);
        }

        [HttpPost]
        [Route("Pwd"), MapToApiVersion("1.0")]
        public async Task<IActionResult> ResetPwd_V1_0([FromBody]SysUser model)
        {
            model.LoginPwd = SecurityHelper.EncryptDES(model.LoginPwd);
            var count = await SysUserRepository.UpdatePwd_V1_0(model);
            var result = new CommandResult<int> { Data = count };

            return Ok(result);
        }

    }
}
