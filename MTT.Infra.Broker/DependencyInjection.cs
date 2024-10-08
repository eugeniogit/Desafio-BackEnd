using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTT.Domain.Shared;
using MTT.Infra.Broker.RabbitMqBroker;

namespace MTT.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddBroker(this IServiceCollection services, IConfiguration configuration)
		{
			var rabbitMqConfig = configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>();
			services.AddSingleton(rabbitMqConfig);
			services.AddSingleton<IMessageBroker, RabbitMqBroker>();
			return services;

		}
	}
}
