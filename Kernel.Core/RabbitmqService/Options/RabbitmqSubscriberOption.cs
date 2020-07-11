using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.RabbitmqService.Options
{
    public class RabbitmqSubscriberOption
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool MultipleAck { get; set; } = false; //手动确认消费
        public ushort PrefetchCount { get; set; } = 4;
        public int StartRetryInterval { get; set; } = 5;
        public string[] Queues { get; set; }
        public TargetCommand[] Routes { get; set; }
    }

    public class TargetCommand
    {
        public string Target { get; set; }
        public string Command { get; set; }
    }
}
