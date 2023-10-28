using Core.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace webApi.Test.Repository
{
    public class CategoryRepositoryTests
    {
        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new DataContext(options);
            dbContext.Database.EnsureCreated();

            if (await dbContext.Categories.CountAsync() <= 0)
            {
                dbContext.Categories.Add(new Category { Id = 1, Name = "Category 1" });
                dbContext.Categories.Add(new Category { Id = 2, Name = "Category 2" });
                await dbContext.SaveChangesAsync();
            }

            return dbContext;
        }

        [Fact]
        public async void CategoryRepository_CategoryExists_ReturnsTrue()
        {
            // Arrange
            var categoryId = 1;
            var dbContext = await GetDatabaseContext();
            var categoryRepository = new CategoryRepository(dbContext);

            // Act
            var result = categoryRepository.CategoryExists(categoryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void CategoryRepository_GetCategory_ReturnsCategory()
        {
            // Arrange
            var categoryId = 1;
            var dbContext = await GetDatabaseContext();
            var categoryRepository = new CategoryRepository(dbContext);

            // Act
            var result = categoryRepository.GetCategory(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        public async void CategoryRepository_GetCategories_ReturnsListOfCategories()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var categoryRepository = new CategoryRepository(dbContext);

            // Act
            var result = categoryRepository.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Category>>(result);
        }

        [Fact]
        public async void CategoryRepository_CreateCategory_ReturnsTrue()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var categoryRepository = new CategoryRepository(dbContext);

            var category = new Category { Name = "New Category" };

            // Act
            var result = categoryRepository.CreateCategory(category);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void CategoryRepository_UpdateCategory_ReturnsTrue()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var categoryRepository = new CategoryRepository(dbContext);

            var category = dbContext.Categories.FirstOrDefault();

            // Act
            var result = categoryRepository.UpdateCategory(category);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void CategoryRepository_DeleteCategory_ReturnsTrue()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var categoryRepository = new CategoryRepository(dbContext);

            var category = dbContext.Categories.FirstOrDefault();

            // Act
            var result = categoryRepository.DeleteCategory(category);

            // Assert
            Assert.True(result);
        }
    }
}

