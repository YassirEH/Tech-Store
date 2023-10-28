//using Microsoft.AspNetCore.Mvc.Testing;
//using System.Net;
//using Xunit;

//namespace webApi.IntegrityTests
//{
//    public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
//    {
//        private readonly WebApplicationFactory<Program> _factory;
//        private readonly HttpClient _client;

//        public ProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
//        {
//            _factory = factory;
//            _client = _factory.CreateClient();
//        }

//        [Fact]
//        public async Task GetProducts_ReturnsOk()
//        {
//            // Arrange
//            var response = await _client.GetAsync("/api/Product/Get All");

//            // Assert
//            response.EnsureSuccessStatusCode();
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//        }
//    }
//}
