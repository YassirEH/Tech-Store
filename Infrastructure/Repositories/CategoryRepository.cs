using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRep
    {
        private DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(e => e.Id == id);
        }

        public ICollection<Category> FilterByName()
        {
            return _context.Categories.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public bool CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        
    }
}
