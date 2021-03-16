using Kernel.Core.RabbitmqService;
using Kernel.MediatR.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Configure
{
    public static class IServiceCollectionExtensions
    {
        public static void AddBuildinRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            if(Convert.ToBoolean(configuration.GetSection("RabbitMQ:Enabled").Value))
            {
                Console.WriteLine("RabbitMQ is Enabled");
                services.AddRabbitmqPublisher();
                services.AddHostedService<HandlingHostedService>();
            }
            else
                Console.WriteLine("RabbitMQ is Disabled");

        }

    }
}
