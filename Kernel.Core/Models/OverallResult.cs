using Kernel.Core.Utils;
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
        public bool success { get; set; } = true;

        /// <summary>
        /// 执行结果信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 错误类别代码（自动化测试可以用于区分错误类别）
        /// </summary>
        public int errCode { get; set; } = OverallErrCode.ERR_NO;

        /// <summary>
        /// 执行结果资源编码（文案资源映射）
        /// </summary>
        public int resCode { get; set; } = 1;//ResponseCode.SUCCESS
        /// <summary>
        /// 请求的API版本号
        /// </summary>
        public string apiVersion { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public int timestamp { get; set; } = DateHelper.DateTimeToStamp(DateTime.Now);
        /// <summary>
        /// 响应具体数据，支持复杂类型
        /// </summary>
        public T data { get; set; }
    }
}
