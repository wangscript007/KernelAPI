using Dapper;
using Newtonsoft.Json;
using System;

namespace Kernel.Model.Demo
{


    /// <summary>
    /// 适用场景：要忽略的属性较多时
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SysUserExt2 : SysUser
    {
        /// <summary>
        /// 用户表主键ID
        /// </summary>
        [Key]
        [Column("USER_ID")]
        [JsonProperty]
        public override string UserID { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [Column("USER_LOGIN")]
        [JsonProperty]
        public override string UserLogin { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("USER_NAME")]
        [JsonProperty]
        public override string UserName { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Column("USER_EMAIL")]
        [JsonProperty]
        public override string UserEmail { get; set; }
    }


}

