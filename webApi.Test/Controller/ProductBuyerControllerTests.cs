using AutoMapper;
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
    public class ProductBuyerControllerTests
    {
        private readonly ProductBuyerController _controller;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IProductBuyerRep> _productBuyerRepMock;
        private readonly Mock<IBuyerRep> _buyerRepMock;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public ProductBuyerControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _productBuyerRepMock = new Mock<IProductBuyerRep>();
            _buyerRepMock = new Mock<IBuyerRep>();
            _notificationServiceMock = new Mock<INotificationService>();

            _controller = new ProductBuyerController(
                _productBuyerRepMock.Object,
                _mapperMock.Object,
                _buyerRepMock.Object,
                _notificationServiceMock.Object);
        }

        [Fact]
        public void AssignProductToBuyer_ValidInput_ReturnsOkResponse()
        {
            // Arrange
            int buyerId = 1;
            int productId = 2;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(true);

            // Act
            var result = _controller.AssignProductToBuyer(buyerId, productId);

            // Assert
            result.Should().BeOfType<OkResult>();
            _productBuyerRepMock.Verify(repo => repo.AssignProductToBuyer(buyerId, productId), Times.Once);
            _notificationServiceMock.Verify(service => service.Notify("Product assigned to buyer", "Success", ErrorType.Success), Times.Once);
        }

        [Fact]
        public void AssignProductToBuyer_InvalidBuyer_ReturnsNotFoundResponse()
        {
            // Arrange
            int buyerId = 1;
            int productId = 2;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(false);

            // Act
            var result = _controller.AssignProductToBuyer(buyerId, productId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _notificationServiceMock.Verify(service => service.Notify("Buyer not found", "Error", ErrorType.NotFound), Times.Once);
        }

        [Fact]
        public void AssignProductToBuyer_InvalidProductId_ReturnsBadRequestResponse()
        {
            // Arrange
            int buyerId = 1;
            int productId = -1;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(true);

            // Act
            var result = _controller.AssignProductToBuyer(buyerId, productId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _notificationServiceMock.Verify(service => service.Notify("Invalid product ID provided", "Error", ErrorType.Error), Times.Once);
        }
    }
}
