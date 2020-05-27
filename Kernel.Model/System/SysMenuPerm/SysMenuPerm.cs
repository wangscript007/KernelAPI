using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.System
{
    /// <summary>
    /// 菜单权限
    /// </summary>
    [Table("sysmenuperm")]
    public class SysMenuPerm
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Key]
        [Column("RoleID")]
        public virtual string RoleID { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        [Key]
        [Column("ModID")]
        public virtual string ModID { get; set; }

    }
}
