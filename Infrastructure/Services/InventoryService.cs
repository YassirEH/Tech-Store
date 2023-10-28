using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly DataContext _dbContext;

        public InventoryService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetCurrentStock(int productId)
        {
            var product = _dbContext.Products.Find(productId);
            return product.Stock;
        }

        public bool IsProductInStock(int productId, int requiredQuantity)
        {
            int currentStock = GetCurrentStock(productId);
            return currentStock >= requiredQuantity;
        }

        public void AddStock(int productId, int adjustmentQuantity)
        {
            var product = _dbContext.Products.Find(productId);
            product.Stock += adjustmentQuantity;
            _dbContext.SaveChanges();
        }

    }
}
