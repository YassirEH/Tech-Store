using Core.Models;

namespace Core.Interfaces
{
    public interface IProductRep
    {
        public Product GetProduct(int id);
        public Product GetProduct(string name);
        public ICollection<Product> GetProducts();
        public bool ProductExists(int productId);
        public bool CreateProduct(Product product, int categoryId);
        public bool UpdateProduct(int productId, Product updatedProduct);
        public bool DeleteProduct(Product product);
        public ICollection<Product> FilterByPrice();
        public ICollection<Product> FilterByName();
        public ICollection<Product> FilterByQuantity();
    }
}
