/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020-04-24       初始版本

******************************************************************/
using Kernel.IService.Repository.Core;
using Kernel.Model.Core;
using Kernel.Model.Core.Attachment;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Core.Attachment.V1_0
{
    public class GetFileCommandHandler : IRequestHandler<GetFileCommand, CommandResult<SysAttachmentsOutParams>>
    {
        IAttachmentRepository _fileRepository;

        public GetFileCommandHandler(IAttachmentRepository apiRepository)
        {
            _fileRepository = apiRepository;
        }

        public Task<CommandResult<SysAttachmentsOutParams>> Handle(GetFileCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var result = await _fileRepository.GetAttachment_V1_0<SysAttachmentsOutParams>(request.Data.AttachId);

                return new CommandResult<SysAttachmentsOutParams>() { Data = result };


            });

        }

    }
}
