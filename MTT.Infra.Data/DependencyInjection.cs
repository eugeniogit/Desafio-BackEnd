using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MTT.Infra.Data.Rental;
using MTT.Domain.Rental.Repositories;
using MTT.Infra.Data.Rental.Repositories;
using MTT.Domain.Rental;

namespace MTT.Infra.Data
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Rental"); ;

			services.AddDbContext<RentalDbContext>(options => options.UseNpgsql(connectionString));
			services.AddScoped<IRentalUnitOfWork, RentalDbContext>();
			services.AddScoped<IRentalRepository, RentalRepository>();
			services.AddScoped<IMotoRepository, MotoRepository>();
			services.AddScoped<IMotoboyRepository, MotoboyRepository>();

			return services;

		}

	}
}
