using Dapper;
using Newtonsoft.Json;

namespace Kernel.Model.Demo
{

    /// <summary>
    /// 适用场景：要忽略的属性较少时
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SysUserExt1 : SysUser
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Column("USER_NAME")]
        [JsonProperty("sysUserName")]
        public override string UserName { get; set; }

        [Column("USER_EMAIL")]
        [JsonIgnore]
        public override string UserEmail { get; set; }

        [NotMapped]
        public string orgName { get; set; }
    }

}

