/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020-04-24       初始版本

******************************************************************/
using Kernel.Model.Core;
using Kernel.Model.Core.Attachment;
using MediatR;
using System.Collections.Generic;

namespace Kernel.MediatR.Core.Attachment.V1_0
{
    public class GetFileCommand : IRequest<CommandResult<SysAttachmentsOutParams>>
    {
        /// <summary>
        /// 调用参数
        /// </summary>
        public SysAttachmentsInParams Data { get; private set; }

        public GetFileCommand(SysAttachmentsInParams data)
        {
            Data = data;
        }

    }
}
