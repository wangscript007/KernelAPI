using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.Core.Attachment
{
    /// <summary>
    /// 附件信息表
    /// </summary>
    [Table("sys_files")]
    public class SysAttachments : IDBModel
    {

        /// <summary>
        /// 附件表ID
        /// </summary>
        [Key]
        [Column("ATTACH_ID")]
        public virtual string AttachId { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Column("ATTACH_REMARKS")]
        public virtual string AttachRemarks { get; set; }


        /// <summary>
        /// 业务表数据记录ID（主键ID对应的值）
        /// </summary>
        [Column("ATTACH_BIZ_ID")]
        public virtual string AttachBizId { get; set; }


        /// <summary>
        /// 文件名
        /// </summary>
        [Column("ATTACH_FILENAME")]
        public virtual string AttachFileName { get; set; }


        /// <summary>
        /// 文件类型
        /// </summary>
        [Column("ATTACH_FILETYPE")]
        public virtual string AttachFileType { get; set; }


        /// <summary>
        /// 文件路径（使用相对路径）
        /// </summary>
        [Column("ATTACH_FILEPATH")]
        public virtual string AttachFilepath { get; set; }


        /// <summary>
        /// 文件物理地址
        /// </summary>
        [Column("ATTACH_PHYADDRESS")]
        public virtual string AttachPhyaddress { get; set; }


        /// <summary>
        /// 业务表名
        /// </summary>
        [Column("ATTACH_BIZTABLE")]
        public virtual string AttachBiztable { get; set; }


        /// <summary>
        /// 业务名称
        /// </summary>
        [Column("ATTACH_BIZNAME")]
        public virtual string AttachBizname { get; set; }


        /// <summary>
        /// 字典项--是否有效（0：否  1：是）
        /// </summary>
        [Column("DICT_IS_ENABLED")]
        public virtual string DictIsEnabled { get; set; }


        /// <summary>
        /// 字典项--输入接口（1:电脑 2:手机）
        /// </summary>
        [Column("DICT_INPUT_INTERFACE")]
        public virtual string DictInputInterface { get; set; }


        /// <summary>
        /// 文件大小（单位：字节）
        /// </summary>
        [Column("ATTACH_FILESIZE")]
        public virtual decimal AttachFilesize { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("OP_CREATE_DATE")]
        public virtual DateTime? OpCreateDate { get; set; }


        /// <summary>
        /// 创建人（UID）
        /// </summary>
        [Column("OP_CREATE_USER")]
        public virtual string OpCreateUser { get; set; }


        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("OP_MODIFY_DATE")]
        public virtual DateTime? OpModifyDate { get; set; }


        /// <summary>
        /// 修改人（UID）
        /// </summary>
        [Column("OP_MODIFY_USER")]
        public virtual string OpModifyUser { get; set; }


    }
}