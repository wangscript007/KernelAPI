using Kernel.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.Models
{
    public class OverallResult<T>
    {
        /// <summary>
        /// 是否成功标志(true:成功 false: 不成功)
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 响应具体数据，支持复杂类型
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        /// <summary>
        /// 执行结果信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// 是否是开发环境
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDevelopment { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public int Timestamp { get; set; } = DateHelper.DateTimeToStamp(DateTime.Now);


    }
}
