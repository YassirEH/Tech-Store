using Core.Models;

namespace Core.Interfaces
{
    public interface ICategoryRep
    {
        public bool CategoryExists(int id);
        public Category GetCategory(int id);
        public ICollection<Category> FilterByName();
        public ICollection<Category> GetCategories();
        public bool CreateCategory(Category category);
        public bool UpdateCategory(Category category);
        public bool DeleteCategory(Category category);

    }
}
