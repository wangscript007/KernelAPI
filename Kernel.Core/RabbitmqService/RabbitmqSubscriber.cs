using Kernel.Core.RabbitmqService.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.Core.RabbitmqService
{
    public interface IMessageSubscriber : IDisposable
    {
        Task SubscribeAsync<TMessage>(string topic, Func<TMessage, Task> handler);
    }

    public class RabbitmqSubscriber : IMessageSubscriber
    {
        RabbitmqSubscriberOption options = null;

        IConnection connection = null;
        IModel channel = null;

        Type handlingType { get; set; }
        Func<object, Task> handler { get; set; }

        public bool ConnectionOpened => connection != null && connection.IsOpen;
        public bool ChannelOpened => channel != null && channel.IsOpen;

        public RabbitmqSubscriber(RabbitmqSubscriberOption options)
        {
            this.options = options;
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ Connection Shutdown");
        }

        private void Connection_RecoverySucceeded(object sender, EventArgs e)
        {
            Console.WriteLine("RabbitMQ Connection Recovery Succeeded");
        }

        private void Connection_ConnectionRecoveryError(object sender, ConnectionRecoveryErrorEventArgs e)
        {
            Console.WriteLine("RabbitMQ Connection Recovery Error");
        }

        private void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ Consumer Shutdown");
        }

        private void StartConnection()
        {
            if (ConnectionOpened)
            {
                connection.Close();
            }

            connection = new ConnectionFactory()
            {
                HostName = options.HostName,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password,
                RequestedHeartbeat = TimeSpan.FromSeconds(10), //心跳时间
                AutomaticRecoveryEnabled = true //自动恢复连接
            }.CreateConnection();

            connection.ConnectionShutdown += Connection_ConnectionShutdown;

            Console.WriteLine("RabbitMQ Connection Successfully");

        }

        private void StartChannel()
        {
            if (ChannelOpened)
            {
                channel.Close();
            }

            StartConnection();

            channel = connection.CreateModel();
            channel.BasicQos(0, options.PrefetchCount, false);

            Console.WriteLine("RabbitMQ Channel Successfully");
        }

        private void StartConsumer()
        {
            StartChannel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Shutdown += Consumer_Shutdown;
            consumer.Received += async (sender, e) =>
            {
                await ReceiveMessage(sender, e);
            };

            //消费配置的队列
            foreach (var queue in options.Queues)
            {
                //先声明队列
                channel.QueueDeclare(
                  queue: queue,//消息队列名称
                  durable: true,//是否缓存
                  exclusive: false,
                  autoDelete: false,
                  arguments: null
                   );

                //通过上面的队列声明，可以避免队列不存在时报错
                channel.BasicConsume(queue, false, consumer);
            }

            Console.WriteLine("RabbitMQ Consumer Successfully");

        }
        private void Start()
        {
            if (ConnectionOpened && ChannelOpened)
            {
                Console.WriteLine("RabbitMQ Connection and Channel is open");

                return;
            }

            try
            {
                StartConsumer();
                Console.WriteLine("RabbitMQ Start Successfully");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
                Thread.Sleep(TimeSpan.FromSeconds(options.StartRetryInterval));

                Start();
            }
        }

        public Task StartAsync()
        {
            return Task.Run(() => Start());
        }

        private async Task<bool> HandleMessage(object message)
        {
            try
            {
                await handler(message);
                Console.WriteLine("RabbitMQ Handle Message Completed");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
                return false;
            }
        }
        private async Task ReceiveMessage(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine("RabbitMQ Receive Message Completed");

            var success = false;
            var message = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(e.Body.ToArray()), handlingType);
            success = await HandleMessage(message);

            if (success)
            {
                //确认消费
                channel.BasicAck(e.DeliveryTag, options.MultipleAck);
                Console.WriteLine("RabbitMQ Reply Message Completed");

            }
            else
            {
                //拒收后重新放入队列（第二个参数是否requeue，true则重新入队列，否则丢弃或者进入死信队列。）
                //如果既不确认消费，也不拒收，则消息会变成Unacked状态，直到连接断开后，变为Ready状态，重连时又可以消费
                channel.BasicReject(e.DeliveryTag, true);
                Console.WriteLine("RabbitMQ Reject Message Completed");
                Thread.Sleep(TimeSpan.FromSeconds(options.StartRetryInterval));
            }
        }

        public Task SubscribeAsync<TMessage>(string topic, Func<TMessage, Task> handler)
        {
            handlingType = typeof(TMessage);
            this.handler = (message) =>
            {
                if (message is TMessage)
                {
                    return handler((TMessage)message);
                }
                else
                {
                    throw new Exception("message type error");
                }
            };

            Console.WriteLine("RabbitMQ Subscribe Message Completed");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (ChannelOpened)
            {
                channel.Close();
            }

            if (ConnectionOpened)
            {
                connection.Close();
            }
        }
    }
}
