using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Kernel.Core.RabbitmqService.Options;

namespace Kernel.Core.RabbitmqService
{
    public class HandlingHostedService : BackgroundService
    {
        readonly RabbitmqSubscriber _subscriber;
        private IMediator _mediator;
        RabbitmqSubscriberOption _subscriberOption;

        public HandlingHostedService(
            IMediator mediator,
            IOptions<RabbitmqSubscriberOption> subscriberOption
            )
        {
            _subscriber = new RabbitmqSubscriber(subscriberOption.Value);
            _mediator = mediator;
            _subscriberOption = subscriberOption.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _subscriber.SubscribeAsync<Message<JToken>>("#", async (message) =>
                {
                    var option = _subscriberOption.Routes.FirstOrDefault(o => o.Target == message.Head.Target);
                    if (option != null)
                    {
                        object[] parameters = new object[] { message };
                        object command = Assembly.GetExecutingAssembly().CreateInstance(option.Command, true, BindingFlags.Default, null, parameters, null, null);
                        var result = await _mediator.Send(command);
                    }
                    else
                    {
                        throw new Exception("未找到该消息的处理程序！");
                    }

                }).Wait();

                await _subscriber.StartAsync();

                Console.WriteLine("托管服务启动完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
