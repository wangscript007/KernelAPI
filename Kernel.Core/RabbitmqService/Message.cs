/*****************************************************************
版本         创建/修改人        时间                描述/修改内容
====         =========       ========         ==================================
1.0           张晓松          2020年7月9日10:20:13       初始版本

******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.RabbitmqService
{
    ///<summary>
    /// 版 本：v1.0.0
    /// 创建人：张晓松
    /// 日 期：2020年7月9日10:20:20
    /// 描 述：消息队列参数
    ///</summary>
    public class Message<TEntity>
    {
        /// <summary>
        /// 请求的Head
        /// </summary>
        public Head Head { get; set; }

        /// <summary>
        /// 请求的Body
        /// </summary>
        public TEntity Body { get; set; }

    }

    public class Head
    {
        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// route key（决定了消息写入到哪个队列）
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// 消息的处理标识（指定该消息由谁来消费）
        /// </summary>
        public string Target { get; set; }
    }
}
