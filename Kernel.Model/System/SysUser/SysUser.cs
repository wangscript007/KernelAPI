using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.System
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    [Table("sysuser")]
    public class SysUser : IDBModel
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("UserID")]
        public virtual string UserID { get; set; }


        /// <summary>
        /// 登录帐号
        /// </summary>
        [Column("LoginID")]
        public virtual string LoginID { get; set; }


        /// <summary>
        /// 用户名
        /// </summary>
        [Column("UserName")]
        public virtual string UserName { get; set; }


        /// <summary>
        /// 拼音首字母
        /// </summary>
        [Column("UserNameFirstLetter")]
        public virtual string UserNameFirstLetter { get; set; }


        /// <summary>
        /// 密码
        /// </summary>
        [Column("LoginPwd")]
        public virtual string LoginPwd { get; set; }


        /// <summary>
        /// 是否锁定：0、未锁定，1、锁定
        /// </summary>
        [Column("IsLocked")]
        public virtual string IsLocked { get; set; }


        /// <summary>
        /// 职务
        /// </summary>
        [Column("UserJob")]
        public virtual string UserJob { get; set; }


        /// <summary>
        /// 性别：M、男，F、女
        /// </summary>
        [Column("DictGender")]
        public virtual string DictGender { get; set; }


        /// <summary>
        /// 生日
        /// </summary>
        [Column("Birthday")]
        public virtual DateTime Birthday { get; set; }


        /// <summary>
        /// 电话
        /// </summary>
        [Column("TelPhone")]
        public virtual string TelPhone { get; set; }


        /// <summary>
        /// 手机
        /// </summary>
        [Column("MobilePhone")]
        public virtual string MobilePhone { get; set; }


        /// <summary>
        /// 邮件
        /// </summary>
        [Column("Email")]
        public virtual string Email { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Column("Description")]
        public virtual string Description { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        [Column("UserPhoto")]
        public virtual string UserPhoto { get; set; }


        /// <summary>
        /// 是否启用，0、禁用，1、启用
        /// </summary>
        [Column("DictIsActive")]
        public virtual string DictIsActive { get; set; }


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
