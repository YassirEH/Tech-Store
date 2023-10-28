using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class BuyerRepository : IBuyerRep
    {
        private readonly DataContext _context;

        public BuyerRepository(DataContext context)
        {
            _context = context;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool BuyerExists(int id)
        {
            return _context.Buyers.Any(b => b.Id == id);
        }

        public Buyer GetBuyerById(int id)
        {
            return _context.Buyers.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Buyer> FilterByName()
        {
            return _context.Buyers.OrderBy(b => b.LName).ToList();
        }

        public ICollection<Buyer> GetBuyers()
        {
            return _context.Buyers.ToList();
        }

        public bool CreateBuyer(Buyer buyer)
        {
            _context.Buyers.Add(buyer);
            return Save();
        }

        public bool UpdateBuyer(Buyer buyer)
        {
            var existingBuyer = _context.Buyers.FirstOrDefault(b => b.Id == buyer.Id);
            existingBuyer!.FName = buyer.FName;
            existingBuyer.LName = buyer.LName;
            existingBuyer.Email = buyer.Email;

            return Save();
        }

        public bool DeleteBuyer(Buyer buyer)
        {
            _context.Remove(buyer);
            return Save();
        }
    }
}