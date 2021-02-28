using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using Winton.Extensions.Configuration.Consul;

namespace Kernel.Core.Utils
{
    public class ConsulHelper
    {
        public static string ServiceName { get; set; }
        public static string ServiceIP { get; set; }
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

            if (!string.IsNullOrWhiteSpace(consulOption.ServiceName))
                ServiceName = consulOption.ServiceName;
            else if (!string.IsNullOrWhiteSpace(Env.KERNEL_SERVICE_NAME))
                ServiceName = Env.KERNEL_SERVICE_NAME;
            else
                ServiceName = "KernelAPI";

            if (!string.IsNullOrWhiteSpace(configuration["ip"]))
                ServiceIP = configuration["ip"];
            else if (!string.IsNullOrWhiteSpace(consulOption.ServiceIP))
                ServiceIP = consulOption.ServiceIP;
            else
                ServiceIP = KernelApp.Settings.ServiceAddress;

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
            Console.WriteLine($"ServiceIP：{ServiceIP}");
            Console.WriteLine($"ServicePort：{ServicePort}");
            Console.WriteLine($"ServiceTags：{ServiceTags}");


            consulClient.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = $"{ServiceName}@{ServiceIP}:{ServicePort}",//唯一
                Name = ServiceName,//服务名字
                Address = ServiceIP,//注册的ip地址
                Port = int.Parse(ServicePort),//注册的端口号
                Tags = ServiceTags.Split(',', StringSplitOptions.RemoveEmptyEntries),//利用标签传入命令行参数              
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(12),//几秒检查一次
                    HTTP = $"http://{ServiceIP}:{ServicePort}/api/v1/Core/HealthCheck",
                    Timeout = TimeSpan.FromSeconds(3),//多长时间没返回就算失败
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5)//失败以后几秒回收 
                }
            });
        }
    }

    public class ConsulCfg
    {
        public IConfigurationBuilder Config { get; set; }
        public string Folder { get; set; }

        public void AddConsul(string fileName)
        {
            string key = $"{Env.KERNEL_SERVICE_NAME}/{Folder}/{fileName}";
            Console.WriteLine(key);
            KernelApp.Log.Info(key);

            Config.AddConsul(
                key,
                options =>
                {
                    options.Optional = true;
                    options.ReloadOnChange = true;
                    options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                    options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(Env.KERNEL_CONFIG_CENTER); };
                }
            );
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
