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
        public string HavePerm { get; set; }

    }
}
