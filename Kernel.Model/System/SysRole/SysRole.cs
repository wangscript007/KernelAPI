using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.System
{
    /// <summary>
    /// 角色信息
    /// </summary>
    [Table("sysrole")]
    public class SysRole : IDBModel
    {

        /// <summary>
        /// 角色ID
        /// </summary>
        [Key]
        [Column("RoleID")]
        public virtual string RoleID { get; set; }


        /// <summary>
        /// 角色名称
        /// </summary>
        [Column("RoleName")]
        public virtual string RoleName { get; set; }


        /// <summary>
        /// 状态，0、作废，1、在用
        /// </summary>
        [Column("DictIsActive")]
        public virtual string DictIsActive { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Column("Description")]
        public virtual string Description { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy")]
        public virtual string CreateBy { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateTime")]
        public virtual DateTime? CreateTime { get; set; }


        /// <summary>
        /// 修改人
        /// </summary>
        [Column("UpdateBy")]
        public virtual string UpdateBy { get; set; }


        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("UpdateTime")]
        public virtual DateTime? UpdateTime { get; set; }


    }
}
