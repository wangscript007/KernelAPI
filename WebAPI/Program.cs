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
                    //    options.Listen(IPAddress.Loopback, 5000);//本机端口访问
                    //    options.ListenAnyIP(83, opts => opts.UseHttps("res/server.pfx", "zzz123"));//允许远程访问，添加https证书
                    //});
                    //config.UseKestrel().UseUrls("http://*:5000;https://*:5001");
                    config.UseKestrel().UseUrls("http://*:5000");
                })
                //将默认ServiceProviderFactory指定为AutofacServiceProviderFactory
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())                
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((context, config) =>
                    {
                        //注：在linux下，文件路径是区分大小写的
                        config.SetBasePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("Settings/Builtin.json", true, true)
                        .AddJsonFile("Settings/Tenant.json", true, true);
                    });
                    webBuilder.UseStartup<Startup>();
                });
           
    }
}
