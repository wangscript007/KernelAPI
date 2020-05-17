using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.System
{
    /// <summary>
    /// 系统模块
    /// </summary>
    [Table("sysmodule")]
    public class SysModule : IDBModel
    {

        /// <summary>
        /// 模块ID
        /// </summary>
        [Column("ModID")]
        public virtual string ModID { get; set; }


        /// <summary>
        /// 模块名称
        /// </summary>
        [Column("ModName")]
        public virtual string ModName { get; set; }


        /// <summary>
        /// 导航链接
        /// </summary>
        [Column("NavUrl")]
        public virtual string NavUrl { get; set; }


        /// <summary>
        /// 图标
        /// </summary>
        [Column("Image")]
        public virtual string Image { get; set; }


        /// <summary>
        /// 目标位置
        /// </summary>
        [Column("Target")]
        public virtual string Target { get; set; }


        /// <summary>
        /// 模块分类（home、logo、menu）
        /// </summary>
        [Column("ModType")]
        public virtual string ModType { get; set; }


        /// <summary>
        /// 父模块ID
        /// </summary>
        [Column("ModPID")]
        public virtual string ModPID { get; set; }


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
