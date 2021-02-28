using Kernel.Core.Models;
using Kernel.Core.RabbitmqService.Options;
using Kernel.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Buildin.Service
{
    public static class OptionExtensions
    {
        public static void AddBuildinOption(this IServiceCollection services, IConfiguration configuration)
        {
            //配置
            services.Configure<OpenApiInfo>(configuration.GetSection("Swagger"));

            //注册 Microsoft.AspNetCore.Http.IHttpContextAccessor
            services.AddHttpContextAccessor();

            //服务端点配置
            services.Configure<EndpointOption>(configuration.GetSection("Endpoints"));

            //RabbitMQ配置
            services.Configure<RabbitmqPublisherOption>(configuration.GetSection("RabbitMQ"));
            services.Configure<RabbitmqSubscriberOption>(configuration.GetSection("RabbitMQ"));

            //consul配置
            services.Configure<ConsulOption>(configuration.GetSection("Consul"));
        }
    }
}
