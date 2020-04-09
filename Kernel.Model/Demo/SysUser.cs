using Dapper;
using Kernel.Core.Basic;
using System;

namespace Kernel.Model.Demo
{
    [Table("SYS_USER")]
    public class SysUser : IDBModel
    {
        /// <summary>
        /// 用户表主键ID
        /// </summary>
        [Key]
        [Column("USER_ID")]
        public virtual string UserID { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [Column("USER_LOGIN")]
        public virtual string UserLogin { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("USER_NAME")]
        public virtual string UserName { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Column("USER_EMAIL")]
        public virtual string UserEmail { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        [Column("MOBILE_PHONE")]
        public virtual string MobilePhone { get; set; }

        /// <summary>
        /// 组织机构代码-所属单位
        /// </summary>
        [Column("ORG_ID")]
        public virtual string OrgID { get; set; }

        /// <summary>
        /// 用户旧密码（修改密码时用到）
        /// </summary>
        [NotMapped]
        public virtual string OldUserPass { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Column("USER_PASS")]
        public virtual string UserPass { get; set; }

        /// <summary>
        /// 字典项--是否有效[是否锁定帐户同一个功能]（0：否 1：是）
        /// </summary>
        [Column("DICT_IS_ENABLED")]
        public virtual string DictIsEnabled { get; set; }

        /// <summary>
        /// 字典项--是否可编辑（0：否 1：是）
        /// </summary>
        [Column("DICT_ALLOWEDIT")]
        public virtual string DictAllowEdit { get; set; }

        /// <summary>
        /// 字典项--是否可删除（0：否 1：是）
        /// </summary>
        [Column("DICT_ALLOWEDEL")]
        public virtual string DictAllowDel { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("USER_DESC")]
        public virtual string UserDesc { get; set; }

        /// <summary>
        /// 字典项-删除标识
        /// </summary>
        [Column("DICT_DEL_FLAG")]
        public virtual string DictDelFlag { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("OP_CREATE_DATE")]
        public virtual DateTime? OpCreateDate { get; set; }

        /// <summary>
        /// 创建人（UID）
        /// </summary>
        [Column("OP_CREATE_USER")]
        public virtual string OpCreateUser { get; set; }

        /// <summary>
        /// 修改人（UID）
        /// </summary>
        [Column("OP_MODIFY_USER")]
        public virtual string OpModifyUser { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("OP_MODIFY_DATE")]
        public virtual DateTime? OpModifyDate { get; set; }

        /// <summary>
        /// 密码错误次数
        /// </summary>
        private int? _PwdFailures;
        [Column("PWD_FAILURES")]
        public virtual int? PwdFailures
        {
            get { return _PwdFailures == null ? 0 : _PwdFailures; }
            set { _PwdFailures = value; }
        }

        /// <summary>
        /// 是否锁定[登录失败达到累计次数时锁定]（0：否 1：是）
        /// </summary>
        [Column("DICT_IS_LOCKED")]
        public virtual string DictIsLocked { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        [NotMapped]
        public virtual DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 用户下次登录前必须修改密码
        /// </summary>
        [Editable(false)]
        [Column("DICT_UPDPWD_NEXT")]
        public virtual string DictUpdpwdNext { get; set; }

    }
}

