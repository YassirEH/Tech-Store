using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace webApi.IntegrityTests
{
    [TestClass]
    public class ProductControllerIntegrationTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        public ProductControllerIntegrationTests()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [TestInitialize]
        public void Initialize()
        {
            _client = _factory.CreateClient();
        }

        [TestMethod]
        public async Task GetProducts_ReturnsOk()
        {
            // Arrange
            var response = await _client.GetAsync("/api/Product/Get All");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        // You can add more integration tests for other actions in the ProductController
    }
}
