using Dapper;
using Kernel.Model.Core.Attachment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.System
{
    public class SysUserDetail : SysUser
    {
        [NotMapped]
        public SysAttachmentsOutParams Attach { get; set; }
    }
}
