/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020-04-24       初始版本

******************************************************************/
using Kernel.IService.Repository.Core;
using Kernel.Model.Core;
using Kernel.Model.Core.Attachment;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Core.Attachment.V1_0
{
    public class GetFilesCommandHandler : IRequestHandler<GetFilesCommand, CommandResult<IEnumerable<SysAttachmentsOutParams>>>
    {
        IAttachmentRepository _fileRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public GetFilesCommandHandler(IAttachmentRepository apiRepository, IHostingEnvironment hostingEnvironment)
        {
            _fileRepository = apiRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public Task<CommandResult<IEnumerable<SysAttachmentsOutParams>>> Handle(GetFilesCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var list = await _fileRepository.GetAttachmentList_V1_0(request.Data.AttachBizId);

                return new CommandResult<IEnumerable<SysAttachmentsOutParams>>() { data = list };


            });

        }

    }
}
