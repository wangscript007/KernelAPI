using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.Core.Attachment
{

    [JsonObject(MemberSerialization.OptIn)]
    public class SysAttachmentsOutParams : SysAttachments
    {
        /// <summary>
        /// 附件表ID
        /// </summary>
        [Key]
        [JsonProperty("fileId")]
        [Column("ATTACH_ID")]
        public override string AttachId { get; set; }

        /// <summary>
        /// 文件链接
        /// </summary>
        [NotMapped]
        [JsonProperty]
        public string FileUrl { get => KernelApp.Request.FileBaseUrl + AttachPhyaddress; }

        /// <summary>
        /// 文件名
        /// </summary>
        [JsonProperty("fileName")]
        [Column("ATTACH_FILENAME")]
        public override string AttachFileName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [JsonProperty("fileType")]
        [Column("ATTACH_FILETYPE")]
        public override string AttachFileType { get; set; }

    }
}
