using AutoMapper;
using Core.Dto;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoryController : APIController
    {
        private readonly IMapper _mapper;
        private readonly IProductCategoryRep _productCategoryRep;

        public ProductCategoryController(IProductCategoryRep productCategoryRep, IMapper mapper, INotificationService notificationService)
            : base(notificationService)
        {
            _mapper = mapper;
            _productCategoryRep = productCategoryRep;
        }

        [HttpGet("Product/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProductByCategory(int categoryId)
        {
            var products = _mapper.Map<List<ProductDto>>(_productCategoryRep.GetProductByCategory(categoryId));

            if (!ModelState.IsValid)
            {
                _notificationService.Notify("Invalid input", "Error", ErrorType.Error);
                return BadRequest(ModelState);
            }

            return Response(products);
        }

        [HttpGet("Category/{productId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetCategoryByProduct(int productId)
        {
            var categories = _mapper.Map<List<CategoryDto>>(_productCategoryRep.GetCategoryByProduct(productId));

            if (!ModelState.IsValid)
            {
                _notificationService.Notify("Invalid input", "Error", ErrorType.Error);
                return BadRequest(ModelState);
            }

            return Response(categories);
        }
    }
}
