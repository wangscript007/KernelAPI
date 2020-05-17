using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Model.System
{
    public class SysModuleInit
    {
        public SysModuleHomeInfo HomeInfo { get; set; }
        public SysModuleLogoInfo LogoInfo { get; set; }
        public IEnumerable<SysModuleMenuInfo> MenuInfo { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SysModuleHomeInfo : SysModule
    {
        [JsonProperty("title")]
        public override string ModName { get; set; }

        [JsonProperty("href")]
        public override string NavUrl { get; set; }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SysModuleLogoInfo : SysModule
    {
        [JsonProperty("title")]
        public override string ModName { get; set; }

        [JsonProperty("image")]
        public override string Image { get; set; }

        [JsonProperty("href")]
        public override string NavUrl { get; set; }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SysModuleMenuInfo : SysModule
    {
        [JsonProperty("title")]
        public override string ModName { get; set; }

        [JsonProperty("icon")]
        public override string Image { get; set; }

        [JsonProperty("href")]
        public override string NavUrl { get; set; }

        [JsonProperty("target")]
        public override string Target { get; set; }

        [JsonProperty]
        public IEnumerable<SysModuleMenuInfo> Child { get; set; }

    }
}
