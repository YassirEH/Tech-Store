using Core.Dto;
using Core.Interfaces;
using Core.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;
using webApi.Controllers;

namespace webApi.Test
{
    public class CategoryControllerTests
    {
        private readonly ICategoryRep _categoryRepMock;
        private readonly IMapper _mapperMock;
        private readonly CategoryController _controller;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public CategoryControllerTests()
        {
            _categoryRepMock = A.Fake<ICategoryRep>();
            _mapperMock = A.Fake<IMapper>();
            _notificationServiceMock = new Mock<INotificationService>();
            _controller = new CategoryController(_categoryRepMock, _mapperMock, _notificationServiceMock.Object);
        }

        [Fact]
        public void GetCategories_ReturnsOkResult()
        {
            // Arrange
            var categories = new List<Category>();
            var categoryDtos = new List<CategoryDto>();
            A.CallTo(() => _categoryRepMock.GetCategories()).Returns(categories);
            A.CallTo(() => _mapperMock.Map<List<CategoryDto>>(A<IEnumerable<Category>>._)).Returns(categoryDtos);

            // Act
            var result = _controller.GetCategories();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetCategory_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category();
            var categoryDto = new CategoryDto();
            A.CallTo(() => _categoryRepMock.CategoryExists(categoryId)).Returns(true);
            A.CallTo(() => _categoryRepMock.GetCategory(categoryId)).Returns(category);
            A.CallTo(() => _mapperMock.Map<CategoryDto>(A<Category>._)).Returns(categoryDto);

            // Act
            var result = _controller.GetCategory(categoryId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void CreateCategory_ValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var categoryDto = new CategoryDto();
            var category = new Category();
            var createdCategoryDto = new CategoryDto();

            A.CallTo(() => _mapperMock.Map<Category>(categoryDto)).Returns(category);
            A.CallTo(() => _categoryRepMock.CreateCategory(category)).Returns(true);
            A.CallTo(() => _mapperMock.Map<CategoryDto>(category)).Returns(createdCategoryDto);

            // Act
            var result = _controller.CreateCategory(categoryDto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public void UpdateCategory_ExistingIdAndValidData_ReturnsNoContent()
        {
            // Arrange
            var categoryId = 1;
            var updatedCategory = new CategoryDto();
            var existingCategory = new Category();

            A.CallTo(() => _categoryRepMock.GetCategory(categoryId)).Returns(existingCategory);

            // Act
            var result = _controller.UpdateCategory(categoryId, updatedCategory);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public void CreateCategory_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var categoryDto = new CategoryDto();
            _controller.ModelState.AddModelError("key", "error message");

            // Act
            var result = _controller.CreateCategory(categoryDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void UpdateCategory_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var categoryId = 1;
            var updatedCategory = new CategoryDto();
            _controller.ModelState.AddModelError("key", "error message");

            // Act
            var result = _controller.UpdateCategory(categoryId, updatedCategory);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void UpdateCategory_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var categoryId = 1;
            A.CallTo(() => _categoryRepMock.GetCategory(categoryId)).Returns(null);

            // Act
            var result = _controller.UpdateCategory(categoryId, A.Dummy<CategoryDto>());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void DeleteCategory_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var categoryId = 1;
            var existingCategory = new Category();

            A.CallTo(() => _categoryRepMock.CategoryExists(categoryId)).Returns(true);
            A.CallTo(() => _categoryRepMock.GetCategory(categoryId)).Returns(existingCategory);
            A.CallTo(() => _categoryRepMock.DeleteCategory(existingCategory)).Returns(true);

            // Act
            var result = _controller.DeleteCategory(categoryId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void DeleteCategory_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var categoryId = 1;
            A.CallTo(() => _categoryRepMock.CategoryExists(categoryId)).Returns(false);

            // Act
            var result = _controller.DeleteCategory(categoryId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void DeleteCategory_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var categoryId = 1;
            A.CallTo(() => _categoryRepMock.CategoryExists(categoryId)).Returns(true);
            A.CallTo(() => _categoryRepMock.GetCategory(categoryId)).Returns(new Category());
            _controller.ModelState.AddModelError("key", "error message");

            // Act
            var result = _controller.DeleteCategory(categoryId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void DeleteCategory_DeleteFails_ReturnsBadRequest()
        {
            // Arrange
            var categoryId = 1;
            var existingCategory = new Category();

            A.CallTo(() => _categoryRepMock.CategoryExists(categoryId)).Returns(true);
            A.CallTo(() => _categoryRepMock.GetCategory(categoryId)).Returns(existingCategory);
            A.CallTo(() => _categoryRepMock.DeleteCategory(existingCategory)).Returns(false);

            // Act
            var result = _controller.DeleteCategory(categoryId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

    }
}