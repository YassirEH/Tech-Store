using Core.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace webApi.Test.Repository
{
    public class ProductBuyerRepositoryTests
    {
        private readonly DbContextOptions<DataContext> _dbContextOptions;

        public ProductBuyerRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private DataContext GetDatabaseContext()
        {
            var dbContext = new DataContext(_dbContextOptions);
            dbContext.Database.EnsureCreated();
            if (dbContext.Products.Count() <= 0)
            {
                var product = new Product { Id = 1, Name = "Product 1", Description = "Description 1" };
                var buyer = new Buyer { Id = 1, FName = "John", LName = "Doe", Email = "dfgf@dfg.com" };
                dbContext.Products.Add(product);
                dbContext.Buyers.Add(buyer);
                dbContext.ProductBuyers.Add(new ProductBuyer { ProductId = 1, BuyerId = 1 });
                dbContext.SaveChanges();
            }
            return dbContext;
        }

        [Fact]
        public async void ProductBuyerRepository_GetBuyerOfProduct_ReturnsBuyers()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var repository = new ProductBuyerRepository(dbContext);

            // Act
            var buyers = repository.GetBuyerOfProduct(1);

            // Assert
            Assert.NotNull(buyers);
            Assert.NotEmpty(buyers);
            Assert.All(buyers, b => Assert.IsType<Buyer>(b));
        }

        [Fact]
        public async void ProductBuyerRepository_GetProductBuyer_ReturnsProducts()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var repository = new ProductBuyerRepository(dbContext);

            // Act
            var products = repository.GetProductBuyer(1);

            // Assert
            Assert.NotNull(products);
            Assert.NotEmpty(products);
            Assert.All(products, p => Assert.IsType<Product>(p));
        }
    }
}
