namespace Core.Interfaces
{
    public interface IInventoryService
    {
        void AddStock(int productId, int adjustmentQuantity);
        int GetCurrentStock(int productId);
        bool IsProductInStock(int productId, int requiredQuantity);
    }
}
