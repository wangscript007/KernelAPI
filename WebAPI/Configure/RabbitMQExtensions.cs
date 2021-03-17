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
                ColorConsole.WriteEmbeddedColorLine("[darkcyan]RabbitMQ[/darkcyan] is [yellow]Enabled[/yellow]");
                services.AddRabbitmqPublisher();
                services.AddHostedService<HandlingHostedService>();
            }
            else
                ColorConsole.WriteEmbeddedColorLine("[darkcyan]RabbitMQ[/darkcyan] is [yellow]Disabled[/yellow]");

        }

    }
}
