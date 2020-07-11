/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020年7月9日10:21:7       初始版本

******************************************************************/
using Kernel.Core.RabbitmqService;
using Kernel.Model.Core;
using MediatR;
using Newtonsoft.Json.Linq;

namespace Kernel.MediatR.Core.Queue.V1_0
{
    ///<summary>
    /// 版 本：v1.0.0
    /// 创建人：张晓松
    /// 日 期：2020年7月9日10:21:12
    /// 描 述：消息队列发布命令
    public class PublishQueueMsgCommand : IRequest<CommandResult<bool>>
    {
        /// <summary>
        /// 调用参数
        /// </summary>
        public Message<JToken> Data { get; private set; }

        public PublishQueueMsgCommand(Message<JToken> data)
        {
            Data = data;
        }

    }
}
