/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020年7月9日10:24:21       初始版本

******************************************************************/
using Kernel.Core.RabbitmqService;
using Kernel.MediatR.Core.Queue.V1_0;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace WebAPI.Areas.Core.Controllers
{
    ///<summary>
    /// 版 本：v1.0.0
    /// 创建人：张晓松
    /// 日 期：2020年7月9日10:26:0
    /// 描 述：消息队列控制器类
    ///</summary>
    [ApiVersion("1.0")]
    [Authorize]
    public class QueueController : CoreBaseController
    {
        private IMediator _mediator;

        public QueueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 消息队列（发布）
        /// </summary>
        [HttpPost]
        [Route("Queue"), MapToApiVersion("1.0")]
        public async Task<IActionResult> PublishQueueMessage_V1_0([FromBody] Message<JToken> model)
        {
            var result = await _mediator.Send(new PublishQueueMsgCommand(model));
            return Ok(result);
        }

    }
}
