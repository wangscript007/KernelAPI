using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.System
{
    /// <summary>
    /// 用户角色关联
    /// </summary>
    [Table("sysrolerelation")]
    public class SysRoleRelation : IDapperColumnMapping
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
        [Column("UserID")]
        public virtual string UserID { get; set; }


        /// <summary>
        /// 角色ID
        /// </summary>
        [Key]
        [Column("RoleID")]
        public virtual string RoleID { get; set; }


    }
}
