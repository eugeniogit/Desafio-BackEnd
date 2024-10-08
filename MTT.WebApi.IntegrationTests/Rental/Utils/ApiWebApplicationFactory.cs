using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;
using MTT.Domain.Shared;
using MTT.Infra.Broker.RabbitMqBroker;
using Moq;
using FluentResults;
using MTT.Infra.Storage;

namespace MTT.WebApi.IntegrationTests.Rental.Utils
{
    internal class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        //public IConfiguration Configuration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var mocker = new AutoMocker();

                mocker.GetMock<IMessageBroker>()
                   .Setup(a => a.SendMessage(It.IsAny<string>()))
                   .Verifiable();

                var broker = mocker.CreateInstance<RabbitMqBroker>();

                services.AddTransient<IMessageBroker>(m => broker);

                mocker.GetMock<IStorageService>()
                    .Setup(s => s.UploadAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(Result.Ok());

                var storageService = mocker.CreateInstance<S3StorageService>();

                services.AddTransient<IStorageService>(m => storageService);

            });
        }
    }
}