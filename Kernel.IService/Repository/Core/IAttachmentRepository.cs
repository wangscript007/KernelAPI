using Kernel.Model.Core.Attachment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.IService.Repository.Core
{
    public interface IAttachmentRepository
    {
        System.Threading.Tasks.Task AddAttachment_V1_0(SysAttachments attachment);
        System.Threading.Tasks.Task<int> DeleteAttachment_V1_0(string bizID);
        System.Threading.Tasks.Task<int> DeleteAttachment_V1_0(string[] attachIDs);
        System.Threading.Tasks.Task<IEnumerable<SysAttachmentsOutParams>> GetAttachmentList_V1_0(string bizID);
        System.Threading.Tasks.Task<IEnumerable<SysAttachmentsOutParams>> GetAttachmentList_V1_0(string[] attachIDs);
        System.Threading.Tasks.Task<SysAttachments> GetAttachment_V1_0(string attachID);
    }
}
