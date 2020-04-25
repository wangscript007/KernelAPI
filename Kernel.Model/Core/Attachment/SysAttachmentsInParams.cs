using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.Core.Attachment
{
    public class SysAttachmentsInParams : SysAttachments
    {

        public IFormFileCollection Files { get; set; }
    }
}
