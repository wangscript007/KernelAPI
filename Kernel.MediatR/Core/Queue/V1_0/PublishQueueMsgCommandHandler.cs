/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020年7月9日10:22:15       初始版本

******************************************************************/
using Kernel.Core.RabbitmqService;
using Kernel.Model.Core;
using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Core.Queue.V1_0
{
    ///<summary>
    /// 版 本：v1.0.0
    /// 创建人：张晓松
    /// 日 期：2020年7月9日10:22:21
    /// 描 述：消息队列发布命令执行
    ///</summary>
    public class PublishQueueMsgCommandHandler : IRequestHandler<PublishQueueMsgCommand, CommandResult<bool>>
    {
        private IMessagePublisher _publisher;

        public PublishQueueMsgCommandHandler(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public Task<CommandResult<bool>> Handle(PublishQueueMsgCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var msg = request.Data;
                await _publisher.PublishAsync<Message<JToken>>(msg.Head.Exchange, msg.Head.Topic, msg);

                return new CommandResult<bool>() { Data = true };

            });

        }

    }
}
