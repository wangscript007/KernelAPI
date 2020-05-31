using Kernel.IService.Repository.System;
using Kernel.Model.Core;
using Kernel.Model.System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Areas.System.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(Policy = "ApiPerm")]
    public class SysRoleRelationController : SystemBaseController
    {
        private IMediator _mediator;
        public ISysRoleRelationRepository SysRoleRelationRepository { get; set; }

        public SysRoleRelationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("SysRoleRelation/{userID}"), MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSysRoleRelation_V1_0(string userID)
        {
            var data = await SysRoleRelationRepository.GetSysRoleRelation_V1_0(userID);
            var result = new CommandResult<SysRoleRelationSet> { Data = data };
            return Ok(result);
        }

        [HttpPost]
        [Route("SysRoleRelation"), MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveSysRoleRelation_V1_0([FromBody] SysRoleRelationSaveIn model)
        {
            var data = await SysRoleRelationRepository.SaveSysRoleRelation_V1_0(model);
            var result = new CommandResult<bool> { Data = data };
            return Ok(result);
        }

    }
}
