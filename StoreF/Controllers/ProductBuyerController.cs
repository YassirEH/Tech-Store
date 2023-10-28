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
    public class ProductBuyerController : APIController
    {
        private readonly IMapper _mapper;
        private readonly IProductBuyerRep _productBuyerRep;
        private readonly IBuyerRep _buyerRep;

        public ProductBuyerController(IProductBuyerRep productBuyerRep, IMapper mapper, IBuyerRep buyerRep, INotificationService notificationService)
            : base(notificationService)
        {
            _mapper = mapper;
            _productBuyerRep = productBuyerRep;
            _buyerRep = buyerRep;
        }

        [HttpGet("Product/{buyerId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetproductByBuyer(int buyerId)
        {
            var product = _mapper.Map<List<ProductDto>>(_productBuyerRep.GetProductBuyer(buyerId));

            return !ModelState.IsValid ? BadRequest(ModelState) : Response(product);
        }

        [HttpGet("Buyer/{productId}")]
        [ProducesResponseType(200, Type = typeof(Buyer))]
        [ProducesResponseType(400)]
        public IActionResult GetBuyerOfProduct(int productId)
        {
            var buyer = _mapper.Map<List<BuyerDto>>(_productBuyerRep.GetBuyerOfProduct(productId));

            return !ModelState.IsValid ? BadRequest(ModelState) : Response(buyer);
        }

        [HttpPost("{buyerId}/products")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AssignProductToBuyer(int buyerId, [FromBody] int productId)
        {
            if (!_buyerRep.BuyerExists(buyerId))
            {
                _notificationService.Notify("Buyer not found", "Error", ErrorType.NotFound);
                return NotFound();
            }

            if (productId <= 0)
            {
                _notificationService.Notify("Invalid product ID provided", "Error", ErrorType.Error);
                return BadRequest("Invalid product ID provided.");
            }

            _productBuyerRep.AssignProductToBuyer(buyerId, productId);

            _notificationService.Notify("Product assigned to buyer", "Success", ErrorType.Success);
            return Ok();
        }
    }
}
