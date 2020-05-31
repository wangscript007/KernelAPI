using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.System
{
    public class SysRoleListIn : SysRole
    {
        public int Page { get; set; }

        public int Limit { get; set; }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SysRoleListRecord : SysRole
    {
        [Key]
        [JsonProperty]
        public override string RoleID { get; set; }

        [JsonProperty]
        public override string RoleName { get; set; }

        [NotMapped]
        [JsonProperty]
        public string IsActiveName { get; set; }

        [JsonProperty]
        public override string DictIsActive { get; set; }

        [JsonProperty]
        public override string Description { get; set; }
    }
}