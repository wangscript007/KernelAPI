using Autofac.Extensions.DependencyInjection;
using Kernel.Core.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.IO;
using Winton.Extensions.Configuration.Consul;
using System.Threading;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //ServiceHost.Load(host.Services);

            //using (var serviceScope = host.Services.CreateScope())
            //{
            //    var services = serviceScope.ServiceProvider;

            //    try
            //    {
            //        var serviceContext = services.GetRequiredService<IEmailService>();
            //        // Use the context here
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "An error occurred.");
            //    }
            //}

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHost((config) =>
                {
                    //config.UseKestrel(options =>
                    //{
                    //    options.Listen(IPAddress.Loopback, 5000);//�����˿ڷ���
                    //    options.ListenAnyIP(83, opts => opts.UseHttps("res/server.pfx", "zzz123"));//����Զ�̷��ʣ����https֤��
                    //});
                    //config.UseKestrel().UseUrls("http://*:5000;https://*:5001");
                    config.UseKestrel().UseUrls("http://*:39274");
                })
                //��Ĭ��ServiceProviderFactoryָ��ΪAutofacServiceProviderFactory
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((context, config) =>
                    {
                        var folder = $"Settings.{Env.KERNEL_ENVIRONMENT}";
                        string log = "";
                        log = $"Loading >>> {folder}";
                        Console.WriteLine(log);

                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                            File.Copy("Settings.Development/Log4net.config", $"{folder}/Log4net.config");
                        }

                        KernelApp.Log.Info(log);

                        if (string.IsNullOrWhiteSpace(Env.KERNEL_CONFIG_CENTER))
                        {
                            log = "���ļ���������";
                            Console.WriteLine(log);
                            KernelApp.Log.Info(log);

                            //ע����linux�£��ļ�·�������ִ�Сд��
                            config.SetBasePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase)
                            .AddJsonFile($"{folder}/appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"{folder}/Builtin.json", true, true)
                            .AddJsonFile($"{folder}/Tenant.json", true, true)
                            .AddJsonFile($"{folder}/IpRateLimiting.json", true, true);
                        }
                        else
                        {
                            log = "���������ļ�������";
                            Console.WriteLine(log);
                            KernelApp.Log.Info(log);

                            ConsulCfg cfg = new ConsulCfg()
                            {
                                Config = config,
                                Folder = folder
                            };
                            cfg.AddConsul("appsettings.json");
                            cfg.AddConsul("Builtin.json");
                            cfg.AddConsul("Tenant.json");
                            cfg.AddConsul("IpRateLimiting.json");

                        }

                    });
                    webBuilder.UseStartup<Startup>();
                });

    }
}
