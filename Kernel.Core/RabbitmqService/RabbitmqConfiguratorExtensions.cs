using Kernel.Core.RabbitmqService.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kernel.Core.RabbitmqService
{
    public static class RabbitmqConfiguratorExtensions
    {
        public static IServiceCollection AddRabbitmqPublisher(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptions<RabbitmqPublisherOption>>().Value;

            var publisher = new RabbitmqPublisher(options);
            publisher.StartAsync().Wait();

            services.AddSingleton<IMessagePublisher>(publisher);

            return services;
        }
    }
}
