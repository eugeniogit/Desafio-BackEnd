using FluentAssertions;
using MTT.WebApi.IntegrationTests.Rental.Utils;
using System.Net;

namespace MTT.WebApi.IntegrationTests.Rental.Controllers
{
    public class RentalControllerTests : WebApiIntegrationTestBase
	{

        [Fact]
        public async Task GetRental_ShouldReturnNotFound_ForInvalidId()
        {
            // Arrange
            var rentalId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/rentals/{rentalId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
