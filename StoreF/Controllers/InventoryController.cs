using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : APIController
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService, INotificationService notificationService)
            : base(notificationService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("purchase")]
        public IActionResult PurchaseProduct(int productId, int quantity)
        {
            if (_inventoryService.IsProductInStock(productId, quantity))
            {
                _inventoryService.AddStock(productId, -quantity);
                _notificationService.Notify("Purchase successful!", "Success", ErrorType.Success);
                return Response("Purchase successful!");
            }
            else
            {
                _notificationService.Notify("Product is out of stock.", "Error", ErrorType.Error);
                return BadRequest("Product is out of stock.");
            }
        }

        [HttpPost("add-stock")]
        public IActionResult AddStock(int productId, int quantity)
        {
            _inventoryService.AddStock(productId, quantity);
            _notificationService.Notify("Stock added successfully!", "Success", ErrorType.Success);
            return Ok();
        }

        [HttpPost("deduct-stock")]
        public IActionResult DeductStock(int productId, int quantity)
        {
            _inventoryService.AddStock(productId, -quantity);
            _notificationService.Notify($"Deducted {quantity} units of stock for Product ID: {productId}", "Info", ErrorType.Info);
            return Response($"Deducted {quantity} units of stock for Product ID: {productId}");
        }

        [HttpPut("update-stock")]
        public IActionResult UpdateStock(int productId, int newStock)
        {
            int currentStock = _inventoryService.GetCurrentStock(productId);
            int adjustmentQuantity = newStock - currentStock;
            _inventoryService.AddStock(productId, adjustmentQuantity);
            _notificationService.Notify($"Updated stock for Product ID: {productId} to {newStock}", "Info", ErrorType.Info);
            return Response($"Updated stock for Product ID: {productId} to {newStock}");
        }
    }
}
