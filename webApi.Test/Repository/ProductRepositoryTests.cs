using Core.Models;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace webApi.Test.Repository
{
    public class ProductRepositoryTests
    {
        private async Task<DataContext> GetDatabaseContext()
        {
            var option = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new DataContext(option);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Products.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Products.Add(
                        new Product()
                        {
                            Name = "Galaxy A52",
                            Description = "A great mid tier phone (still better than any iphone out there)",
                            ProductCategories = new List<ProductCategory>()
                            {
                                new ProductCategory { Category = new Category() { Name = "Phones"} }
                            }
                        }
                        );
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void ProductRepository_GetProduct_ReturnOk()
        {
            //Assemble
            var name = "Galaxy A52";
            var dbContext = await GetDatabaseContext();
            var productRepository = new ProductRepository(dbContext);

            //Act
            var result = productRepository.GetProduct(name);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);

        }

        [Fact]
        public async void ProductController_ProductExists_ReturnOk()
        {
            int id = 2;
            var dbContext = await GetDatabaseContext();
            var productController = new ProductRepository(dbContext);

            var result = productController.ProductExists(id);

            Assert.True(result);
        }

        [Fact]
        public async void ProductController_ProductDoesNotExist_ReturnsFalse()
        {
            int id = 11;
            var dbContext = await GetDatabaseContext();
            var productRepository = new ProductRepository(dbContext);

            var result = productRepository.ProductExists(id);

            Assert.False(result);
        }

        [Fact]
        public async void ProductRepository_CreateProduct_ReturnsTrue()
        {
            // Assemble
            var dbContext = await GetDatabaseContext();
            var productRepository = new ProductRepository(dbContext);

            var product = new Product()
            {
                Name = "Galaxy A52",
                Description = "A great mid tier phone (still better than any iphone out there)",
                Price = 3400
            };

            var category = new Category()
            {
                Name = "Phones"
            };
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();

            // Act
            var result = productRepository.CreateProduct(product, category.Id);

            // Assert
            Assert.True(result);
            dbContext.Products.Should().ContainEquivalentOf(product);
            dbContext.ProductCategories.Should().Contain(pc =>
                pc.Product == product && pc.Category == category);
        }

        [Fact]
        public async void ProductRepository_UpdateProduct_ReturnOk()
        {
            var productId = 3;
            var dbContext = await GetDatabaseContext();
            var productRepository = new ProductRepository(dbContext);

            var product = new Product()
            {
                Name = "Galaxy A52",
                Description = "A great mid tier phone (still better than any iphone out there)",
                Price = 3400
            };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var updatedProduct = new Product()
            {
                Name = "Galaxy A52",
                Description = "Old Ahh phone must be slow by today's standards",
                Price = 2300
            };
            //act
            var result = productRepository.UpdateProduct(productId, updatedProduct);

            //assert
            Assert.True(result);
        }

        [Fact]
        public async void ProductRepository_DeleteProduct_ReturnOk()
        {
            //Assemble

            var dbContext = await GetDatabaseContext();
            var productRepository = new ProductRepository(dbContext);

            var product = new Product()
            {
                Name = "Galaxy A52",
                Description = "A great mid tier phone (still better than any iphone out there)",
                Price = 3400
            };

            dbContext.Products.Add(product);
            dbContext.SaveChanges();


            //Act
            var result = productRepository.DeleteProduct(product);

            //Assert
            Assert.True(result);
        }
    }
}
