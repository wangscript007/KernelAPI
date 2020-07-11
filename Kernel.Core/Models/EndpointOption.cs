/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020年7月9日15:5:47       初始版本

******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.Models
{
    ///<summary>
    /// 版 本：v1.0.0
    /// 创建人：张晓松
    /// 日 期：2020年7月9日15:6:3
    /// 描 述：服务端点
    ///</summary>
    public class EndpointOption
    {
        /// <summary>
        /// 日志记录服务（登录日志、操作日志）
        /// </summary>
        public string Logging { get; set; }

        /// <summary>
        /// 数据同步服务
        /// </summary>
        public string Sync { get; set; }

        /// <summary>
        /// 短信发送服务
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 邮件发送服务
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 安全认证Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Schema
        /// </summary>
        public string Schema { get; set; }

    }
}
