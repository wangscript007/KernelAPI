using Kernel.Core.AOP;
using Kernel.Core.RabbitmqService.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.Core.RabbitmqService
{
    public interface IMessagePublisher : IDisposable
    {
        Task PublishAsync<TMessage>(string exchange, string topic, TMessage message);
    }

    public class RabbitmqPublisher : IMessagePublisher
    {
        RabbitmqPublisherOption options = null;

        IConnection connection = null;
        IModel channel = null;
        IBasicProperties properties = null;

        public event EventHandler<BasicReturnEventArgs> MessageRouteError;

        public bool ConnectionOpened => connection != null && connection.IsOpen;
        public bool ChannelOpened => channel != null && channel.IsOpen;

        public RabbitmqPublisher(RabbitmqPublisherOption options)
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

        private void Channel_BasicReturn(object sender, BasicReturnEventArgs e)
        {
            Console.WriteLine("RabbitMQ Unroutable Message");
            MessageRouteError?.Invoke(sender, e);
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
                RequestedHeartbeat = TimeSpan.FromSeconds(10),
                AutomaticRecoveryEnabled = true
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
            channel.BasicReturn += Channel_BasicReturn;

            properties = channel.CreateBasicProperties();
            properties.Persistent = options.Persistent;

            foreach (var exchange in options.Exchanges)
            {
                //声明交换机   通配符类型为topic
                channel.ExchangeDeclare(exchange: exchange.Name, type: "topic", durable: true);

                foreach (var bind in exchange.Binding)
                {
                    //声明队列
                    channel.QueueDeclare(queue: bind.Queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    //将队列与交换机进行绑定
                    channel.QueueBind(queue: bind.Queue, exchange: exchange.Name, routingKey: bind.RoutingKey);

                }
            }


            Console.WriteLine("RabbitMQ Channel Successfully");
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
                StartChannel();

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

        private Task<bool> SendMessage<TMessage>(string exchange, string topic, TMessage message)
        {
            channel.BasicPublish(
                exchange: exchange,
                routingKey: topic,
                basicProperties: properties,
                body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)),
                //body: MessagePackSerializer.NonGeneric.Serialize(typeof(TMessage), message),
                mandatory: true
            );

            Console.WriteLine("RabbitMQ Send Message Completed");
            return Task.FromResult(true);
        }

        public async Task PublishAsync<TMessage>(string exchange, string topic, TMessage message)
        {
            //判断交换机配置是否存在
            var exists = options.Exchanges.Any(o => o.Name == exchange);
            if (!exists)
            {
                throw new KernelException($"未找到交换机『{exchange}』的配置信息！");
            }

            if(!ConnectionOpened || !ChannelOpened)
                Start();

            await SendMessage(exchange, topic, message);

        }

        public void Dispose()
        {
            if (ChannelOpened)
            {
                channel?.Close();
            }

            if (ConnectionOpened)
            {
                connection.Close();
            }
        }
    }
}
