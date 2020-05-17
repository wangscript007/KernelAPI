using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.System
{

    [JsonObject(MemberSerialization.OptIn)]
    public class SysUserLogin : SysUser
    {
        [JsonProperty]
        public override string UserID { get; set; }

        [JsonProperty]
        public override string LoginID { get; set; }

        [JsonProperty]
        public override string UserName { get; set; }

        [NotMapped]
        [JsonProperty]
        public string Token { get; set; }

    }
}
