using Core.Dto;
using Core.Interfaces;
using Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;
using webApi.Controllers;

namespace webApi.Test.Controller
{
    public class BuyerControllerTests
    {
        private readonly BuyerController _controller;
        private readonly Mock<IBuyerRep> _buyerRepMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public BuyerControllerTests()
        {
            _buyerRepMock = new Mock<IBuyerRep>();
            _mapperMock = new Mock<IMapper>();
            _notificationServiceMock = new Mock<INotificationService>();

            _controller = new BuyerController(_buyerRepMock.Object, _mapperMock.Object, _notificationServiceMock.Object);
        }

        [Fact]
        public void BuyerController_GetBuyer_ReturnsOk()
        {
            // Arrange
            var buyers = new List<Buyer>();
            _buyerRepMock.Setup(rep => rep.GetBuyers()).Returns(buyers);
            _mapperMock.Setup(mapper => mapper.Map<List<BuyerDto>>(buyers)).Returns(new List<BuyerDto>());

            // Act
            var result = _controller.GetBuyer();

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
        }

        [Fact]
        public void BuyerController_GetBuyerById_ExistingId_ReturnsOk()
        {
            // Arrange
            var buyerId = 1;
            var buyer = new Buyer();
            _buyerRepMock.Setup(rep => rep.BuyerExists(buyerId)).Returns(true);
            _buyerRepMock.Setup(rep => rep.GetBuyerById(buyerId)).Returns(buyer);
            _mapperMock.Setup(mapper => mapper.Map<BuyerDto>(buyer)).Returns(new BuyerDto());

            // Act
            var result = _controller.GetBuyerById(buyerId);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
        }

        [Fact]
        public void BuyerController_GetBuyerById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var buyerId = 1;
            _buyerRepMock.Setup(rep => rep.BuyerExists(buyerId)).Returns(false);

            // Act
            var result = _controller.GetBuyerById(buyerId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void BuyerController_FilterByName_ReturnsOk()
        {
            // Arrange
            var buyers = new List<Buyer>();
            _buyerRepMock.Setup(rep => rep.FilterByName()).Returns(buyers);
            _mapperMock.Setup(mapper => mapper.Map<List<BuyerDto>>(buyers)).Returns(new List<BuyerDto>());

            // Act
            var result = _controller.FilterByName();

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
        }

        [Fact]
        public void BuyerController_CreateBuyer_ValidModel_ReturnsCreatedAtAction()
        {
            // Arrange
            var buyerDto = new BuyerDto();
            var buyer = new Buyer();
            _mapperMock.Setup(mapper => mapper.Map<Buyer>(buyerDto)).Returns(buyer);
            _buyerRepMock.Setup(rep => rep.CreateBuyer(buyer));
            _mapperMock.Setup(mapper => mapper.Map<BuyerDto>(buyer)).Returns(buyerDto);

            // Act
            var result = _controller.CreateBuyer(buyerDto);

            // Assert
            result.Should().NotBeNull().And.BeOfType<CreatedAtActionResult>();
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.ActionName.Should().Be(nameof(BuyerController.GetBuyerById));
        }

        [Fact]
        public void BuyerController_UpdateBuyer_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var buyerId = 1;
            var buyerDto = new BuyerDto();
            var existingBuyer = new Buyer();
            _buyerRepMock.Setup(rep => rep.BuyerExists(buyerId)).Returns(true);
            _buyerRepMock.Setup(rep => rep.GetBuyerById(buyerId)).Returns(existingBuyer);

            // Act
            var result = _controller.UpdateBuyer(buyerId, buyerDto);

            // Assert
            result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        }

        [Fact]
        public void BuyerController_UpdateBuyer_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var buyerId = 1;
            var buyerDto = new BuyerDto();
            _buyerRepMock.Setup(rep => rep.BuyerExists(buyerId)).Returns(false);

            // Act
            var result = _controller.UpdateBuyer(buyerId, buyerDto);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void BuyerController_DeleteBuyer_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var buyerId = 1;
            _buyerRepMock.Setup(rep => rep.BuyerExists(buyerId)).Returns(true);
            var existingBuyer = new Buyer();
            _buyerRepMock.Setup(rep => rep.GetBuyerById(buyerId)).Returns(existingBuyer);
            _buyerRepMock.Setup(rep => rep.DeleteBuyer(existingBuyer)).Returns(true);

            // Act
            var result = _controller.DeleteBuyer(buyerId);

            // Assert
            result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        }

        [Fact]
        public void BuyerController_DeleteBuyer_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var buyerId = 1;
            _buyerRepMock.Setup(rep => rep.BuyerExists(buyerId)).Returns(false);

            // Act
            var result = _controller.DeleteBuyer(buyerId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void BuyerController_DeleteBuyer_FailedToDelete_ReturnsBadRequestWithNotification()
        {
            // Arrange
            var buyerId = 1;
            _buyerRepMock.Setup(rep => rep.BuyerExists(buyerId)).Returns(true);
            var existingBuyer = new Buyer();
            _buyerRepMock.Setup(rep => rep.GetBuyerById(buyerId)).Returns(existingBuyer);
            _buyerRepMock.Setup(rep => rep.DeleteBuyer(existingBuyer)).Returns(false);

            // Act
            var result = _controller.DeleteBuyer(buyerId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _notificationServiceMock.Verify(service => service.Notify("Error deleting the buyer", "Error", ErrorType.Error), Times.Once);
        }
    }
}
