using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRep
    {
        private readonly DataContext _context;

        public ProductCategoryRepository(DataContext context)
        {
            _context = context;
        }

        public int ProductId { get; internal set; }

        public ICollection<Category> GetCategoryByProduct(int productId)
        {
            return _context.ProductCategories.Where(p=>p.ProductId == productId).Select(w=>w.Category).ToList();
        }

        public ICollection<Product> GetProductByCategory(int categoryId)
        {
            return _context.ProductCategories.Where(e => e.CategoryId == categoryId).Select(c => c.Product).ToList();
        }
    }
}
