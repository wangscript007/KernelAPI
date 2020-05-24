using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.System
{
    /// <summary>
    /// Api资源
    /// </summary>
    [Table("apiresource")]
    public class ApiResource : IDBModel
    {

        /// <summary>
        /// 资源ID
        /// </summary>
        [Key]
        [Column("ResID")]
        public virtual string ResID { get; set; }


        /// <summary>
        /// 资源名称
        /// </summary>
        [Column("ResName")]
        public virtual string ResName { get; set; }


        /// <summary>
        /// 资源路径
        /// </summary>
        [Column("ResPath")]
        public virtual string ResPath { get; set; }


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


    }
}
