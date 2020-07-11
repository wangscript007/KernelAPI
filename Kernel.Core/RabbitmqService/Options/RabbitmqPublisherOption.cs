using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.RabbitmqService.Options
{
    public class RabbitmqPublisherOption
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Persistent { get; set; } = true;

        public int StartRetryInterval { get; set; } = 5;

        public IEnumerable<Exchange> Exchanges { get; set; }
    }

    /// <summary>
    /// 交换机
    /// </summary>
    public class Exchange
    {
        public string Name { get; set; }

        public IEnumerable<Binding> Binding { get; set; }

    }

    /// <summary>
    /// 路由绑定
    /// </summary>
    public class Binding
    {
        public string RoutingKey { get; set; }

        public string Queue { get; set; }

    }

}
