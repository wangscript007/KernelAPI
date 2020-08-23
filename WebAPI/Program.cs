using Autofac.Extensions.DependencyInjection;
using Kernel.Core.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;

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
                .ConfigureWebHost((config) => {
                    //config.UseKestrel(options =>
                    //{
                    //    options.Listen(IPAddress.Loopback, 5000);//�����˿ڷ���
                    //    options.ListenAnyIP(83, opts => opts.UseHttps("res/server.pfx", "zzz123"));//����Զ�̷��ʣ����https֤��
                    //});
                    //config.UseKestrel().UseUrls("http://*:5000;https://*:5001");
                    //config.UseKestrel().UseUrls("http://*:5000");
                })
                //��Ĭ��ServiceProviderFactoryָ��ΪAutofacServiceProviderFactory
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())                
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((context, config) =>
                    {
                        //ע����linux�£��ļ�·�������ִ�Сд��
                        config.SetBasePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("Settings/Builtin.json", true, true)
                        .AddJsonFile("Settings/Tenant.json", true, true)
                        .AddJsonFile("Settings/IpRateLimiting.json", true, true);
                    });
                    webBuilder.UseStartup<Startup>();
                });
           
    }
}
