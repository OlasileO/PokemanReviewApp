using PokemanReviewApp.Interfaces;
using PokemanReviewApp.Model;

namespace PokemanReviewApp.Repository
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CategoryExist(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            _context.SaveChanges();
            return true;    
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            _context.SaveChanges();
            return true;
        }

        public ICollection<Category> GetCategories()
        {
           return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int id)
        {
           return _context.PokemonCategories.Where( c=> c.CategoryId== id).Select(c=>c.Pokemon).ToList();
        }

        public bool UpadateCategory(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();
            return true;
        }
    }
}
