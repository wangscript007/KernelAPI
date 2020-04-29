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
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Core.Attachment.V1_0
{
    public class UploadFilesCommandHandler : IRequestHandler<UploadFilesCommand, CommandResult<List<SysAttachmentsOutParams>>>
    {
        IAttachmentRepository _fileRepository;

        public UploadFilesCommandHandler(IAttachmentRepository apiRepository)
        {
            _fileRepository = apiRepository;
        }

        public Task<CommandResult<List<SysAttachmentsOutParams>>> Handle(UploadFilesCommand request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var files = request.Data.Files;
                long size = files.Sum(f => f.Length);
                string filePath = $"{DateTime.Now.ToString("yyyy/MM/dd")}/{ request.Data.AttachBizId}/";
                string filePhysicalPath = $"{App.AttachmentPath}{filePath}";  //文件路径  可以通过注入 IHostingEnvironment 服务对象来取得Web根目录和内容根目录的物理路径
                if (!Directory.Exists(filePhysicalPath)) //判断上传文件夹是否存在，若不存在，则创建
                {
                    Directory.CreateDirectory(filePhysicalPath); //创建文件夹
                }

                List<SysAttachmentsOutParams> list = new List<SysAttachmentsOutParams>();

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileID = Guid.NewGuid().ToString("N");
                        var fileType = Path.GetExtension(file.FileName);
                        var fileName = fileID + fileType;//文件名+文件后缀名
                        using (var stream = new FileStream(filePhysicalPath + fileName, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        //写入数据库
                        SysAttachmentsOutParams attachment = new SysAttachmentsOutParams();
                        attachment.AttachId = fileID;
                        attachment.AttachBizId = request.Data.AttachBizId;
                        attachment.AttachFileName = Path.GetFileNameWithoutExtension(file.FileName);
                        attachment.AttachFileType = fileType;
                        attachment.AttachFilepath = filePath;
                        attachment.AttachPhyaddress = filePath + fileName;
                        attachment.AttachBizname = "";
                        attachment.DictIsEnabled = "1";
                        attachment.DictInputInterface = "1";
                        attachment.AttachFilesize = size;
                        attachment.OpCreateDate = DateTime.Now;
                        attachment.OpCreateUser = "";
                        attachment.OpModifyDate = DateTime.Now;
                        attachment.OpModifyUser = "";
                        _fileRepository.AddAttachment_V1_0(attachment);
                        list.Add(attachment);
                    }
                }

                return new CommandResult<List<SysAttachmentsOutParams>>() { Data = list };

            });

        }

    }
}
