/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020-04-24       初始版本

******************************************************************/
using Kernel.Core;
using Kernel.Core.Utils;
using Kernel.IService.Repository.Core;
using Kernel.Model.Core;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Core.Attachment.V1_0
{
    public class DownloadZipCommandHandler : IRequestHandler<DownloadZipCommand, CommandResult<string>>
    {
        IAttachmentRepository _fileRepository;

        public DownloadZipCommandHandler(IAttachmentRepository apiRepository)
        {
            _fileRepository = apiRepository;
        }

        public Task<CommandResult<string>> Handle(DownloadZipCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var zipedFile = "";

                var files = await _fileRepository.GetAttachmentList_V1_0(request.Data.AttachBizId);
                if (files.Count() != 0)
                {
                    var dict = files.ToDictionary(k => k.AttachId + k.AttachFileType, v => v.AttachFileName + v.AttachFileType);

                    var folderToZip = App.AttachmentPath + files.First().AttachFilepath;
                    zipedFile = "temp/" + DateHelper.DateTimeToStamp(DateTime.Now) + ".zip";

                    //ZipHelper.ZipDirectory(folderToZip, App.BasePath + zipedFile);
                    Compress.DoZipFile(App.AttachmentPath + zipedFile, folderToZip, dict);
                }

                return new CommandResult<string>() { Data = zipedFile };

            });

        }

    }
}
