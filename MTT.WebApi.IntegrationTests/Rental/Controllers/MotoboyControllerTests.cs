using AutoFixture;
using FluentAssertions;
using MTT.Domain.Rental.ValueObjects;
using MTT.WebApi.IntegrationTests.Rental.Utils;
using MTT.WebApi.Rental.Commands;
using System.Net;
using System.Net.Http.Json;

namespace MTT.WebApi.IntegrationTests.Rental.Controllers
{
    public class MotoboyControllerTests : WebApiIntegrationTestBase
	{
		private readonly IFixture _fixture;

		public MotoboyControllerTests()
        {
			_fixture = new Fixture();
		}

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenMotoboyNotExists()
        {
            // Arrange
            var motoboyId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/motoboys/{motoboyId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Add_WhenMotoboyIsAdded()
        {
            // Arrange
            var CNHNumber = _fixture.Create<int>();
			var CNPJ = _fixture.Create<string>();

			var command = new AddMotoboy("John Doe", CNPJ, DateTime.UtcNow, CNHNumber, CNHCategory.A, "base64string");

            // Act
            var response = await _client.PostAsJsonAsync("/motoboys", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenMotoboyIsInvalid()
        {
            // Arrange
            var command = new AddMotoboy(string.Empty, string.Empty, DateTime.UtcNow, 0, CNHCategory.A, string.Empty);

            // Act
            var response = await _client.PostAsJsonAsync("/motoboys", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UploadCNH_ShouldReturnBadRequest_WhenMotoboyIsInvalid()
        {
            // Arrange
            var command = new UploadCNH(Guid.NewGuid().ToString(), "newbase64string");

            // Act
            var response = await _client.PutAsJsonAsync($"/motoboys/{Guid.NewGuid()}/cnh", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
