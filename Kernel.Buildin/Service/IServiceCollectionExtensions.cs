using Kernel.Core.RabbitmqService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class IServiceCollectionExtensions
    {
        public static void AddBuildinRabbitMQ(this IServiceCollection services)
        {
            services.AddRabbitmqPublisher();
            services.AddHostedService<HandlingHostedService>();

        }

    }
}
