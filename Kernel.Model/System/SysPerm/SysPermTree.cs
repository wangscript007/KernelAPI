using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.System
{
    [JsonObject(MemberSerialization.OptOut)]
    public class SysPermTree : SysModule
    {
        [NotMapped]
        public bool HavePerm { get; set; }

        [JsonIgnore]
        public override string Target { get; set; }

        [JsonIgnore]
        public override string CreateBy { get; set; }


        [JsonIgnore]
        public override DateTime? CreateTime { get; set; }


        [JsonIgnore]
        public override string UpdateBy { get; set; }


        [JsonIgnore]
        public override DateTime? UpdateTime { get; set; }

        public IEnumerable<SysFuncPerm> FuncPerms { get; set; }
    }
}
