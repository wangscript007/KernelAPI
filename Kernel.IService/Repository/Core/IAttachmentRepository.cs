using Kernel.Model.Core.Attachment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.Core
{
    public interface IAttachmentRepository
    {
        Task AddAttachment_V1_0(SysAttachments attachment);
        Task<int> DeleteAttachment_V1_0(string bizID);
        Task<int> DeleteAttachment_V1_0(string[] attachIDs);
        Task<IEnumerable<SysAttachmentsOutParams>> GetAttachmentList_V1_0(string bizID);
        Task<IEnumerable<SysAttachmentsOutParams>> GetAttachmentList_V1_0(string[] attachIDs);
        Task<T> GetAttachment_V1_0<T>(string attachID);
        Task<SysAttachments> GetAttachment_V1_0(string attachID);
    }
}
