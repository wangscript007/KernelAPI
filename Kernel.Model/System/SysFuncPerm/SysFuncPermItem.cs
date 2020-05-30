using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.System
{
    public class SysFuncPermItem : SysFuncPerm
    {
        public bool HavePerm { get; set; }

        [JsonIgnore]
        public override string RoleID { get; set; }

        [JsonIgnore]
        public override string CreateBy { get; set; }

        [JsonIgnore]
        public override DateTime? CreateTime { get; set; }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SysModuleFuncPerm : SysFuncPerm
    {
        [JsonProperty]
        public override string FuncCode { get; set; }

        [JsonProperty]
        public bool HavePerm { get; set; }
    }

}
