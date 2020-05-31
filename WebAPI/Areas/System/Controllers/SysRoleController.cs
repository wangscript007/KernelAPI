using Kernel.IService.Repository.System;
using Kernel.Model.Core;
using Kernel.Model.System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Areas.System.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(Policy = "ApiPerm")]
    public class SysRoleController : SystemBaseController
    {
        private IMediator _mediator;
        public ISysRoleRepository SysRoleRepository { get; set; }

        public SysRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("SysRole/{roleID}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSysRole_V1_0(string roleID)
        {
            var user = await SysRoleRepository.GetSysRole_V1_0(roleID);
            var result = new CommandResult<SysRole> { Data = user };

            return Ok(result);
        }

        [HttpGet]
        [Route("SysRole"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSysRoleList_V1_0([FromQuery]SysRoleListIn model)
        {
            var result = await SysRoleRepository.GetSysRoleList_V1_0(model);

            return Ok(result);
        }

        [HttpDelete]
        [Route("SysRole"), MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteSysRole_V1_0([FromQuery]SysRole model)
        {
            var count = await SysRoleRepository.DeleteSysRole_V1_0(model.RoleID.Split(','));
            var result = new CommandResult<int> { Data = count };

            return Ok(result);
        }

        [HttpPost]
        [Route("IsActive"), MapToApiVersion("1.0")]
        public async Task<IActionResult> SetSysRoleIsActive_V1_0([FromBody]SysRole model)
        {
            var role = await SysRoleRepository.GetSysRole_V1_0(model.RoleID);
            role.DictIsActive = model.DictIsActive;
            await SysRoleRepository.UpdateSysRole_V1_0(role);

            var result = new CommandResult<bool> { Data = true };

            return Ok(result);
        }

        [HttpPost]
        [Route("SysRole"), MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveSysRole_V1_0([FromBody]SysRole model)
        {
            if (string.IsNullOrEmpty(model.RoleID))
            {
                model.RoleID = KernelApp.Settings.NewGUID;
                model.CreateBy = KernelApp.Request.UserID;
                model.CreateTime = DateTime.Now;
                model.UpdateBy = KernelApp.Request.UserID;
                model.UpdateTime = DateTime.Now;
                await SysRoleRepository.AddSysRole_V1_0(model);
            }
            else
            {
                model.UpdateBy = KernelApp.Request.UserID;
                model.UpdateTime = DateTime.Now;
                await SysRoleRepository.UpdateSysRole_V1_0(model);
            }

            var result = new CommandResult<string> { Data = model.RoleID };

            return Ok(result);
        }


    }
}
