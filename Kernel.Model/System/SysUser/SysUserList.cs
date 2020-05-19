using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.System
{
    public class SysUserListIn : SysUser
    {
        public int Page { get; set; }

        public int Limit { get; set; }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SysUserListRecord : SysUser
    {
        [Key]
        [JsonProperty]
        public override string UserID { get; set; }


        [JsonProperty]
        public override string LoginID { get; set; }


        [JsonProperty]
        public override string UserName { get; set; }


        [JsonProperty]
        public override string UserJob { get; set; }


        [JsonProperty]
        public override string JobNumber { get; set; }


        [JsonProperty]
        public override string MobilePhone { get; set; }

        [NotMapped]
        [JsonProperty]
        public string IsActiveName { get; set; }

        [JsonProperty]
        public override string Description { get; set; }

    }

}
