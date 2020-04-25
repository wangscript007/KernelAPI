/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020-04-24       初始版本

******************************************************************/
using Kernel.Core;
using Kernel.IService.Repository.Core;
using Kernel.Model.Core;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Core.Attachment.V1_0
{
    public class DeleteFilesCommandHandler : IRequestHandler<DeleteFilesCommand, CommandResult<bool>>
    {
        IAttachmentRepository _fileRepository;

        public DeleteFilesCommandHandler(IAttachmentRepository apiRepository)
        {
            _fileRepository = apiRepository;
        }

        public Task<CommandResult<bool>> Handle(DeleteFilesCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var attachIDs = request.Data.AttachId.Split(',');
                var files = await _fileRepository.GetAttachmentList_V1_0(attachIDs);
                var result = await _fileRepository.DeleteAttachment_V1_0(attachIDs);

                foreach (var file in files)
                {
                    //删除物理文件
                    var path = App.BasePath + "/" + file.AttachPhyaddress;
                    File.Delete(path);
                }

                return new CommandResult<bool>() { data = true };

            });

        }

    }
}
