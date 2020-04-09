using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Demo.HelloWorld.V1_0
{
    public class CNReplyHandler : INotificationHandler<HelloWorldCommand>
    {
        public Task Handle(HelloWorldCommand notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"CN Reply: Hello from CN");
            return Task.CompletedTask;
        }
    }
}
