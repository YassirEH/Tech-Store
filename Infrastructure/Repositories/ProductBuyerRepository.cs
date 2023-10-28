using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ProductBuyerRepository : IProductBuyerRep
    {
        private readonly DataContext _context;

        public ProductBuyerRepository(DataContext context)
        {
            _context = context;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public ICollection<Buyer> GetBuyerOfProduct(int productId)
        {
            return _context.ProductBuyers.Where(pb => pb.ProductId == productId).Select(pb => pb.Buyer).ToList();
        }

        public ICollection<Product> GetProductBuyer(int buyerId)
        {
            return _context.ProductBuyers.Where(pb => pb.BuyerId == buyerId).Select(pb => pb.Product).ToList();
        }

        public bool AssignProuctToBuyer(int buyerId, int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            var productBuyer = new ProductBuyer()
            {
                BuyerId = buyerId,
                ProductId = productId
            };
            _context.ProductBuyers.Add(productBuyer);
            return Save();
        }

        public bool AssignProductToBuyer(int buyerId, int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            var productBuyer = new ProductBuyer()
            {
                BuyerId = buyerId,
                ProductId = productId
            };
            _context.ProductBuyers.Add(productBuyer);
            return Save();
        }
    }
}
