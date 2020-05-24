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
    public class SysModuleController : SystemBaseController
    {
        private IMediator _mediator;
        public ISysModuleService SysModuleService { get; set; }

        public SysModuleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("init"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSysModuleInit_V1_0()
        {
            var result = await SysModuleService.GetSysModuleInit();
            return Ok(result);
        }

        [HttpGet]
        [Route("PermTree"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetPermTree_V1_0()
        {
            var result = new LayuiTableResult<SysPermTree>();
            result.Data = await SysModuleService.GetPermTree();
            result.Count = result.Data.Count();
            return Ok(result);
        }

    }
}
