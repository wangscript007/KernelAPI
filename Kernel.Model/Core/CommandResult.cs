using Kernel.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kernel.Model.Core
{
    /// <summary>
    /// 公共返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandResult<T> : OverallResult<T>
    {
        /// <summary>
        /// 额外信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Extra { get; set; }
    }

    public class LayuiTableResult<T>
    {
        public string Code { get; set; } = "0";
        public string Msg { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }

}