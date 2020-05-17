using Kernel.IService.Service.System;
using Kernel.Model.System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Areas.System.Controllers
{
    [ApiVersion("1.0")]
    [Authorize]
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
        public async Task<IActionResult> GetSysModuleInit_V1_0([FromQuery]SysUser user)
        {
            var result = await SysModuleService.GetSysModuleInit();
            return Ok(result);
        }

    }
}
