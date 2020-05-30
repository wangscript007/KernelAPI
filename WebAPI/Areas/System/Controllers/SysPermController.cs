using Kernel.IService.Repository.System;
using Kernel.IService.Service.System;
using Kernel.Model.Core;
using Kernel.Model.System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Areas.System.Controllers
{
    [ApiVersion("1.0")]
    public class SysPermController : SystemBaseController
    {
        private IMediator _mediator;
        public ISysFuncPermRepository SysFuncPermRepository { get; set; }
        public ISysMenuPermRepository SysMenuPermRepository { get; set; }

        public SysPermController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("SavePerm"), MapToApiVersion("1.0")]
        [Authorize(Policy = "ApiPerm")]
        public async Task<IActionResult> SavePerm_V1_0([FromBody] SysPermSave model)
        {
            var result = new CommandResult<bool> { };
            await SysMenuPermRepository.SaveSysMenuPerm_V1_0(model.RoleID, model.MenuPerms);
            await SysFuncPermRepository.SaveSysFuncPerm_V1_0(model.RoleID, model.FuncPerms);

            return Ok(result);
        }

        [HttpGet]
        [Route("FuncPerm/{modID}"), MapToApiVersion("1.0")]
        [Authorize]
        public async Task<IActionResult> GetFuncPerm_V1_0(string modID)
        {
            var result = new CommandResult<IEnumerable<SysModuleFuncPerm>> { };
            result.Data = await SysFuncPermRepository.GetModuleFuncPerm_V1_0(modID);
            return Ok(result);
        }

    }
}
