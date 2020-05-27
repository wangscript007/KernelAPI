using Kernel.IService.Repository.System;
using Kernel.IService.Service.System;
using Kernel.Model.Core;
using Kernel.Model.System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Areas.System.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(Policy = "ApiPerm")]
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
        public async Task<IActionResult> SavePerm_V1_0([FromBody] SysPermSave model)
        {
            var result = new CommandResult<bool> { Success = false };
            await SysMenuPermRepository.SaveSysMenuPerm_V1_0(model.RoleID, model.MenuPerms);
            await SysFuncPermRepository.SaveSysFuncPerm_V1_0(model.RoleID, model.FuncPerms);

            return Ok(result);
        }

    }
}
