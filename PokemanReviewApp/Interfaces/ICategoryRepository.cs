using PokemanReviewApp.Model;

namespace PokemanReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategory(int id);
        bool CategoryExist (int id);
        bool CreateCategory(Category category);
        bool UpadateCategory(Category category);
        bool DeleteCategory(Category category);


    }
}
