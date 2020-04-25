using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.Core.Attachment
{
    public class SysAttachmentsDownloadParams
    {
        public byte[] Content { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }
    }
}
