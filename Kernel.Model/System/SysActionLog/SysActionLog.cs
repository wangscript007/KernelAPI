using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.System
{
    /// <summary>
    /// action执行记录
    /// </summary>
    [Table("sysactionlog")]
    public class SysActionLog : IDBModel
    {

        /// <summary>
        /// 远程ID
        /// </summary>
        [Key]
        [Column("RemoteIp")]
        public virtual string RemoteIp { get; set; }


        /// <summary>
        /// api名称
        /// </summary>
        [Column("ApiName")]
        public virtual string ApiName { get; set; }


        /// <summary>
        /// action消耗时间
        /// </summary>
        [Column("Elapsed")]
        public virtual double Elapsed { get; set; }


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


    }
}
