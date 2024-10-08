using Microsoft.Extensions.DependencyInjection;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using MTT.Domain.Rental.Services;
using MTT.Domain.Shared;

namespace MTT.Infra.Storage
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDefaultAWSOptions(configuration.GetAWSOptions());
			services.AddAWSService<IAmazonS3>();
			services.AddScoped<IStorageService, S3StorageService>();
			return services;
		}

	}
}
