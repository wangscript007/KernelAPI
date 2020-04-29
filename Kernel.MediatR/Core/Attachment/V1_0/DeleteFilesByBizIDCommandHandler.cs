/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020-04-24       初始版本

******************************************************************/
using Kernel.Core;
using Kernel.Core.Models;
using Kernel.IService.Repository.Core;
using Kernel.Model.Core;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Core.Attachment.V1_0
{
    public class DeleteFilesByBizIDCommandHandler : IRequestHandler<DeleteFilesByBizIDCommand, CommandResult<bool>>
    {
        IAttachmentRepository _fileRepository;

        public DeleteFilesByBizIDCommandHandler(IAttachmentRepository apiRepository)
        {
            _fileRepository = apiRepository;
        }

        public Task<CommandResult<bool>> Handle(DeleteFilesByBizIDCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var files = await _fileRepository.GetAttachmentList_V1_0(request.Data.AttachBizId);
                if (files.Count() != 0)
                {
                    var result = await _fileRepository.DeleteAttachment_V1_0(request.Data.AttachBizId);

                    //删除物理文件夹
                    var path = App.AttachmentPath + files.First().AttachFilepath;
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                }

                return new CommandResult<bool>() { Data = true };

            });

        }

    }
}
