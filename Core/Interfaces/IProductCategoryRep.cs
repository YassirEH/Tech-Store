using Core.Models;

namespace Core.Interfaces
{
    public interface IProductCategoryRep
    {
        ICollection<Product> GetProductByCategory(int categoryId);
        ICollection<Category> GetCategoryByProduct(int productId);
    }
}
