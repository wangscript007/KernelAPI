/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020-04-24       初始版本

******************************************************************/
using Kernel.Core;
using Kernel.IService.Repository.Core;
using Kernel.Model.Core;
using Kernel.Model.Core.Attachment;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Core.Attachment.V1_0
{
    public class DownFileCommandHandler : IRequestHandler<DownFileCommand, CommandResult<SysAttachmentsDownloadParams>>
    {
        IAttachmentRepository _fileRepository;

        public DownFileCommandHandler(IAttachmentRepository apiRepository)
        {
            _fileRepository = apiRepository;
        }

        public Task<CommandResult<SysAttachmentsDownloadParams>> Handle(DownFileCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var file = await _fileRepository.GetAttachment_V1_0(request.Data.AttachId);
                if (file == null)
                    return null;

                var path = App.AttachmentPath + file.AttachPhyaddress;
                if (!File.Exists(path))
                    return null;

                new FileExtensionContentTypeProvider().Mappings.TryGetValue(file.AttachFileType, out var contenttype);

                var param = new SysAttachmentsDownloadParams()
                {
                    Content = File.ReadAllBytes(path),
                    ContentType = contenttype ?? "application/octet-stream",
                    FileName = file.AttachFileName + file.AttachFileType
                };

                return new CommandResult<SysAttachmentsDownloadParams>() { data = param };

            });

        }

    }
}
