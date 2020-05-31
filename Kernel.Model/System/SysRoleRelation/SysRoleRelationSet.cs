using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.System
{
    public class SysRoleRelationSet
    {
        public IEnumerable<SysRoleRelationItem> AllRoles { get; set; }

        public IEnumerable<string> UserRoles { get; set; }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SysRoleRelationItem : SysRoleRelation
    {
        [JsonProperty("value")]
        public override string RoleID { get; set; }

        [JsonProperty("title")]
        public string RoleName { get; set; }

        [JsonProperty]
        public bool Disabled { get; set; }

    }

    public class SysRoleRelationSaveIn
    {
        public int SaveType { get; set; }

        public string UserID { get; set; }

        public IEnumerable<string> RoleIDs { get; set; }

    }

}
