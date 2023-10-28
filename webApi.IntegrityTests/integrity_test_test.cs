using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using webApi.IntegrityTests;
using Xunit;

namespace webApi.IntegrityTests
{
    public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<TestStartup>>
    {
        private readonly WebApplicationFactory<TestStartup> _factory;
        private readonly HttpClient _httpClient;

        public ProductControllerIntegrationTests(WebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_ReturnsOkStatus()
        {
            // Arrange

            // Act
            var response = await _httpClient.GetAsync("/api/Product/Get All");

            // Assert
            response.EnsureSuccessStatusCode(); // Check if the response status code is 2xx
                                                // Perform additional assertions based on the response content
        }

    }
}
