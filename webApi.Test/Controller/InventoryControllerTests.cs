using Core.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using webApi.Controllers;
using webApi.Application.Services;
using Xunit;

namespace webApi.Test.Controller
{
    public class InventoryControllerTests
    {
        private readonly InventoryController _controller;
        private readonly Mock<IInventoryService> _inventoryServiceMock;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public InventoryControllerTests()
        {
            _inventoryServiceMock = new Mock<IInventoryService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _controller = new InventoryController(_inventoryServiceMock.Object, _notificationServiceMock.Object);
        }

        [Fact]
        public void InventoryController_PurchaseProduct_ProductOutOfStock_ReturnsBadRequest()
        {
            // Arrange
            int productId = 2;
            int quantity = 5;
            _inventoryServiceMock.Setup(service => service.IsProductInStock(productId, quantity)).Returns(false);

            // Act
            var result = _controller.PurchaseProduct(productId, quantity);

            // Assert
            result.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be("Product is out of stock.");
            _notificationServiceMock.Verify(service => service.Notify("Product is out of stock.", "Error", ErrorType.Error), Times.Once);
        }

        [Fact]
        public void InventoryController_AddStock_ReturnsSuccessResponse()
        {
            // Arrange
            int productId = 1;
            int quantity = 5;

            // Act
            var result = _controller.AddStock(productId, quantity);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkResult>();
            _inventoryServiceMock.Verify(service => service.AddStock(productId, quantity), Times.Once);
            _notificationServiceMock.Verify(service => service.Notify("Stock added successfully!", "Success", ErrorType.Success), Times.Once);
        }

        [Fact]
        public void InventoryController_DeductStock_ReturnsSuccessResponse()
        {
            // Arrange
            int productId = 2;
            int quantity = 2;

            // Act
            var result = _controller.DeductStock(productId, quantity);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var responseMessage = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value, null);
            responseMessage.Should().Be($"Deducted {quantity} units of stock for Product ID: {productId}");
            _inventoryServiceMock.Verify(service => service.AddStock(productId, -quantity), Times.Once);
            _notificationServiceMock.Verify(service => service.Notify($"Deducted {quantity} units of stock for Product ID: {productId}", "Info", ErrorType.Info), Times.Once);
        }


        [Fact]
        public void InventoryController_UpdateStock_ReturnsSuccessResponse()
        {
            // Arrange
            int productId = 3;
            int newStock = 10;
            _inventoryServiceMock.Setup(service => service.GetCurrentStock(productId)).Returns(5);

            // Act
            var result = _controller.UpdateStock(productId, newStock);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var responseMessage = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value, null);
            responseMessage.Should().Be($"Updated stock for Product ID: {productId} to {newStock}");
            _inventoryServiceMock.Verify(service => service.AddStock(productId, newStock - 5), Times.Once);
            _notificationServiceMock.Verify(service => service.Notify($"Updated stock for Product ID: {productId} to {newStock}", "Info", ErrorType.Info), Times.Once);
        }



    }
}
