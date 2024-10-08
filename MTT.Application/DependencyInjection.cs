using Microsoft.Extensions.DependencyInjection;
using MTT.Application.Rental.Services;
using MTT.Domain.Rental.Services;

namespace MTT.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddScoped<IMotoboyService, MotoboyService>();
			services.AddScoped<IMotoService, MotoService>();
			services.AddScoped<IRentalService, RentalService>();
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
			return services;

		}
	}
}
