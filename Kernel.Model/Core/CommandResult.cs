using Kernel.Core.Models;
using Newtonsoft.Json;

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

}