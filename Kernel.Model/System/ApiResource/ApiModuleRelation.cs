using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.System
{
    /// <summary>
    /// 菜单、api资源关联
    /// </summary>
    [Table("apimodulerelation")]
    public class ApiModuleRelation : IDBModel
    {

        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Column("RmID")]
        public virtual string RmID { get; set; }


        /// <summary>
        /// 菜单ID
        /// </summary>
        [Column("ModID")]
        public virtual string ModID { get; set; }


        /// <summary>
        /// 资源ID
        /// </summary>
        [Column("ResID")]
        public virtual string ResID { get; set; }


    }
}
