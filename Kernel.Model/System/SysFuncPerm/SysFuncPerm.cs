using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.System
{
    /// <summary>
    /// 系统功能权限
    /// </summary>
    [Table("sysfuncperm")]
    public class SysFuncPerm : IDBModel
    {

        /// <summary>
        /// 角色
        /// </summary>
        [Column("RoleID")]
        public virtual string RoleID { get; set; }


        /// <summary>
        /// 菜单ID
        /// </summary>
        [Column("ModID")]
        public virtual string ModID { get; set; }


        /// <summary>
        /// 功能ID
        /// </summary>
        [Column("FuncID")]
        public virtual string FuncID { get; set; }


        /// <summary>
        /// 功能编码
        /// </summary>
        [Column("FuncCode")]
        public virtual string FuncCode { get; set; }


        /// <summary>
        /// 功能名称
        /// </summary>
        [Column("FuncName")]
        public virtual string FuncName { get; set; }


        /// <summary>
        /// api名称
        /// </summary>
        [Column("ApiName")]
        public virtual string ApiName { get; set; }


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
