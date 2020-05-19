using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.Demo
{
    /// <summary>
    /// 系统字典项
    /// </summary>
    [Table("sysdictitem")]
    public class SysDictItem : IDBModel
    {

        /// <summary>
        /// 字典ID
        /// </summary>
        [Column("ItemID")]
        public virtual string ItemID { get; set; }


        /// <summary>
        /// 字典ID
        /// </summary>
        [Column("DictID")]
        public virtual string DictID { get; set; }


        /// <summary>
        /// 字典编码
        /// </summary>
        [Column("DictCode")]
        public virtual string DictCode { get; set; }


        /// <summary>
        /// 字典项编码
        /// </summary>
        [Column("ItemCode")]
        public virtual string ItemCode { get; set; }


        /// <summary>
        /// 字典项名称
        /// </summary>
        [Column("ItemName")]
        public virtual string ItemName { get; set; }


        /// <summary>
        /// 字典项PID
        /// </summary>
        [Column("ItemPID")]
        public virtual string ItemPID { get; set; }


        /// <summary>
        /// 排序码
        /// </summary>
        [Column("SortKey")]
        public virtual decimal? SortKey { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy")]
        public virtual string CreateBy { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateTime")]
        public virtual DateTime CreateTime { get; set; }


        /// <summary>
        /// 修改人
        /// </summary>
        [Column("UpdateBy")]
        public virtual string UpdateBy { get; set; }


        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("UpdateTime")]
        public virtual DateTime UpdateTime { get; set; }


        /// <summary>
        /// 是否启用
        /// </summary>
        [Column("IsEnabled")]
        public virtual string IsEnabled { get; set; }


    }
}
