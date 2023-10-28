using Core.Dto;
using Core.Interfaces;
using Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;
using webApi.Controllers;

namespace webApi.Test.Controller
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRep> _productRepMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public ProductControllerTests()
        {
            _productRepMock = new Mock<IProductRep>();
            _mapperMock = new Mock<IMapper>();
            _notificationServiceMock = new Mock<INotificationService>();
        }

        [Fact]
        public void ProductController_GetProducts_ReturnsOk()
        {
            // Arrange
            var products = new List<Product>();
            _productRepMock.Setup(rep => rep.GetProducts()).Returns(products);
            _mapperMock.Setup(mapper => mapper.Map<List<ProductDto>>(products)).Returns(new List<ProductDto>());

            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object, _notificationServiceMock.Object);

            // Act
            var result = controller.GetProducts();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void ProductController_GetProduct_ExistingId_ReturnsOk()
        {
            // Arrange
            var productId = 1;
            var product = new Product();
            var productDto = new ProductDto();
            _productRepMock.Setup(rep => rep.ProductExists(productId)).Returns(true);
            _productRepMock.Setup(rep => rep.GetProduct(productId)).Returns(product);
            _mapperMock.Setup(mapper => mapper.Map<ProductDto>(product)).Returns(productDto);

            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object, _notificationServiceMock.Object);

            // Act
            var result = controller.GetProduct(productId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void ProductController_GetProduct_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var productId = 1;
            _productRepMock.Setup(rep => rep.ProductExists(productId)).Returns(false);

            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object, _notificationServiceMock.Object);

            // Act
            var result = controller.GetProduct(productId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _notificationServiceMock.Verify(service => service.Notify("Product not found", "Error", ErrorType.NotFound), Times.Once);
        }

        [Fact]
        public void ProductController_FilterByName_ReturnsOk()
        {
            // Arrange
            var products = new List<Product>();
            _productRepMock.Setup(rep => rep.FilterByName()).Returns(products);
            _mapperMock.Setup(mapper => mapper.Map<List<ProductDto>>(products)).Returns(new List<ProductDto>());

            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object, _notificationServiceMock.Object);

            // Act
            var result = controller.FilterByName();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        

        [Fact]
        public void ProductController_CreateProduct_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var productDto = new ProductDto();
            var categoryId = 1;
            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object, _notificationServiceMock.Object);
            controller.ModelState.AddModelError("key", "error message");

            // Act
            var result = controller.CreateProduct(productDto, categoryId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _notificationServiceMock.Verify(service => service.Notify("Invalid product data", "Error", ErrorType.Error), Times.Once);
        }

        [Fact]
        public void ProductController_FilterByPrice_ReturnsOk()
        {
            // Arrange
            var products = new List<Product>();
            _productRepMock.Setup(rep => rep.FilterByPrice()).Returns(products);
            _mapperMock.Setup(mapper => mapper.Map<List<ProductDto>>(products)).Returns(new List<ProductDto>());

            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object, _notificationServiceMock.Object);

            // Act
            var result = controller.FilterByPrice();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void ProductController_FilterByQuantity_ReturnsOk()
        {
            // Arrange
            var products = new List<Product>();
            _productRepMock.Setup(rep => rep.FilterByQuantity()).Returns(products);
            _mapperMock.Setup(mapper => mapper.Map<List<ProductDto>>(products)).Returns(new List<ProductDto>());

            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object, _notificationServiceMock.Object);

            // Act
            var result = controller.FilterByQuantity();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void ProductController_CreateProduct_ValidModel_ReturnsCreatedAtAction()
        {
            // Arrange
            var productDto = new ProductDto();
            var categoryId = 1;
            var product = new Product();
            _mapperMock.Setup(mapper => mapper.Map<Product>(productDto)).Returns(product);
            _productRepMock.Setup(rep => rep.CreateProduct(product, categoryId)).Returns(true);
            _mapperMock.Setup(mapper => mapper.Map<ProductDto>(product)).Returns(productDto);

            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object, _notificationServiceMock.Object);

            // Act
            var result = controller.CreateProduct(productDto, categoryId);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            _notificationServiceMock.Verify(service => service.Notify("A new product has been created", "Success", ErrorType.Success), Times.Once);
        }

        [Fact]
        public void ProductController_UpdateProduct_ValidModel_ReturnsNoContent()
        {
            // Arrange
            var productId = 1;
            var productDto = new ProductDto();
            var product = new Product();
            _productRepMock.Setup(rep => rep.ProductExists(productId)).Returns(true);
            _mapperMock.Setup(mapper => mapper.Map<Product>(productDto)).Returns(product);
            _productRepMock.Setup(rep => rep.UpdateProduct(productId, product)).Returns(true);

            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object, _notificationServiceMock.Object);

            // Act
            var result = controller.UpdateProduct(productId, productDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _notificationServiceMock.Verify(service => service.Notify("Product updated successfully", "Success", ErrorType.Success), Times.Once);
        }

        [Fact]
        public void ProductController_DeleteProduct_ValidId_ReturnsNoContent()
        {
            // Arrange
            var productId = 1;
            var product = new Product();
            _productRepMock.Setup(rep => rep.ProductExists(productId)).Returns(true);
            _productRepMock.Setup(rep => rep.GetProduct(productId)).Returns(product);
            _productRepMock.Setup(rep => rep.DeleteProduct(product)).Returns(true);

            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object, _notificationServiceMock.Object);

            // Act
            var result = controller.DeleteProduct(productId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _notificationServiceMock.Verify(service => service.Notify("A new product has been created", "Success", ErrorType.Success), Times.Once);
        }

    }
}
