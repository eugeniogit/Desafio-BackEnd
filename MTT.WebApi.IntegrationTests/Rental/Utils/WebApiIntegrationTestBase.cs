namespace MTT.WebApi.IntegrationTests.Rental.Utils
{
    public abstract class WebApiIntegrationTestBase
    {
        protected readonly HttpClient _client;

        public WebApiIntegrationTestBase()
        {
            var factory = new ApiWebApplicationFactory();
            _client = factory.CreateClient();
        }
    }
}
