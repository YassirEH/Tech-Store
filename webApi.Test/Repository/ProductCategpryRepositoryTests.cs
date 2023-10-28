using Core.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace webApi.Test.Repository
{
    public class ProductCategoryRepositoryTests
    {
        private readonly DbContextOptions<DataContext> _dbContextOptions;

        public ProductCategoryRepositoryTests()
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
                var category = new Category { Id = 1, Name = "Category 1" };
                dbContext.Products.Add(product);
                dbContext.Categories.Add(category);
                dbContext.ProductCategories.Add(new ProductCategory { ProductId = 1, CategoryId = 1 });
                dbContext.SaveChanges();
            }
            return dbContext;
        }

        [Fact]
        public void ProductCategoryRepository_GetCategoryByProduct_ReturnsCategories()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var repository = new ProductCategoryRepository(dbContext);

            // Act
            var categories = repository.GetCategoryByProduct(1);

            // Assert
            Assert.NotNull(categories);
            Assert.NotEmpty(categories);
            Assert.All(categories, c => Assert.IsType<Category>(c));
        }

        [Fact]
        public void ProductCategoryRepository_GetProductByCategory_ReturnsProducts()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var repository = new ProductCategoryRepository(dbContext);

            // Act
            var products = repository.GetProductByCategory(1);

            // Assert
            Assert.NotNull(products);
            Assert.NotEmpty(products);
            Assert.All(products, p => Assert.IsType<Product>(p));
        }
    }
}
