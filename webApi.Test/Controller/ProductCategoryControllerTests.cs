using AutoMapper;
using Core.Dto;
using Core.Interfaces;
using Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using webApi.Application.Services;
using webApi.Controllers;
using Xunit;

namespace webApi.Test.Controller
{
    public class ProductCategoryControllerTests
    {
        private readonly ProductCategoryController _controller;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IProductCategoryRep> _productCategoryRepMock;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public ProductCategoryControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _productCategoryRepMock = new Mock<IProductCategoryRep>();
            _notificationServiceMock = new Mock<INotificationService>();

            _controller = new ProductCategoryController(
                _productCategoryRepMock.Object,
                _mapperMock.Object,
                _notificationServiceMock.Object);
        }


        [Fact]
        public void GetProductByCategory_InvalidCategoryId_ReturnsBadRequest()
        {
            // Arrange
            int categoryId = -1;
            _controller.ModelState.AddModelError("categoryId", "Invalid category ID");

            // Act
            var result = _controller.GetProductByCategory(categoryId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GetCategoryByProduct_InvalidProductId_ReturnsBadRequest()
        {
            // Arrange
            int productId = -1;
            _controller.ModelState.AddModelError("productId", "Invalid product ID");

            // Act
            var result = _controller.GetCategoryByProduct(productId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
