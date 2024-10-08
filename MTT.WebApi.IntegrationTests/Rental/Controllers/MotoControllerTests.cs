using FluentAssertions;
using MTT.Domain.Rental.ValueObjects;
using MTT.WebApi.IntegrationTests.Rental.Utils;
using MTT.WebApi.Rental.Commands;
using System.Net;
using System.Net.Http.Json;

namespace MTT.WebApi.IntegrationTests.Rental.Controllers
{
    public class MotoControllerTests : WebApiIntegrationTestBase
	{
        [Fact]
        public async Task GetMotos_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/motos");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetMoto_ShouldReturnNotFound_ForInvalidId()
        {
            // Arrange
            var motoId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/motos/{motoId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Add_ShouldReturnCreated()
        {
            // Arrange
            var command = new AddMoto("Tag123", 2021, MotoModel.M1 );

            // Act
            var response = await _client.PostAsJsonAsync("/motos", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

    }
}
