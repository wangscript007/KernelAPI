using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Core.Utils
{
    public class ConsulHelper
    {
        public static string ServiceAddress { get; set; }
        public static string ServicePort { get; set; }
        public static string ServiceTags { get; set; }

        public static void ConsulRegister(IConfiguration configuration)
        {
            var consulOption = ServiceHost.GetService<IOptions<ConsulOption>>().Value;

            if (!consulOption.Enabled)
            {
                Console.WriteLine("未启用consul注册，将跳过注册");
                return;
            }

            ConsulClient consulClient = new ConsulClient(c =>
            {
                c.Address = new Uri(consulOption.Host);
                c.Datacenter = consulOption.Datacenter;
            });

            if (!string.IsNullOrWhiteSpace(configuration["ip"]))
                ServiceAddress = configuration["ip"];
            else if (!string.IsNullOrWhiteSpace(consulOption.ServiceIP))
                ServiceAddress = consulOption.ServiceIP;
            else
                ServiceAddress = KernelApp.Settings.ServiceAddress;

            if (!string.IsNullOrWhiteSpace(configuration["port"]))
                ServicePort = configuration["port"];
            else if (!string.IsNullOrWhiteSpace(consulOption.ServicePort))
                ServicePort = consulOption.ServicePort;
            else
                ServicePort = "80";

            if (!string.IsNullOrWhiteSpace(configuration["tags"]))
                ServiceTags = configuration["tags"];
            else if (!string.IsNullOrWhiteSpace(consulOption.ServiceTags))
                ServiceTags = consulOption.ServiceTags;
            else
                ServiceTags = "";

            Console.WriteLine($"启用consul注册：{consulOption.Host}");
            Console.WriteLine($"ServiceAddress：{ServiceAddress}");
            Console.WriteLine($"ServicePort：{ServicePort}");
            Console.WriteLine($"ServiceTags：{ServiceTags}");


            consulClient.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = $"{consulOption.ServiceName}@{ServiceAddress}:{ServicePort}",//唯一
                Name = consulOption.ServiceName,//服务名字
                Address = ServiceAddress,//注册的ip地址
                Port = int.Parse(ServicePort),//注册的端口号
                Tags = ServiceTags.Split(',', StringSplitOptions.RemoveEmptyEntries),//利用标签传入命令行参数              
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(12),//几秒检查一次
                    HTTP = $"http://{ServiceAddress}:{ServicePort}/api/v1/Core/HealthCheck",
                    Timeout = TimeSpan.FromSeconds(3),//多长时间没返回就算失败
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5)//失败以后几秒回收 
                }
            });
        }
    }

    public class ConsulOption
    {
        public bool Enabled { get; set; }

        public string Datacenter { get; set; }

        public string Host { get; set; }

        public string ServiceName { get; set; }

        public string ServiceIP { get; set; }

        public string ServicePort { get; set; }

        public string ServiceTags { get; set; }
    }

}
