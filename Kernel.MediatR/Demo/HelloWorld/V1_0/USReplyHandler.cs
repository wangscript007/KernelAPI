using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.MediatR.Demo.HelloWorld.V1_0
{
    public class USReplyHandler : INotificationHandler<HelloWorldCommand>
    {
        public Task Handle(HelloWorldCommand notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"US Reply: Hello from US");
            return Task.CompletedTask;
        }
    }
}
