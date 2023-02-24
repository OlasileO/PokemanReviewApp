using Microsoft.OpenApi.Models;
using PokemanReviewApp.Interfaces;
using PokemanReviewApp.Model;

namespace PokemanReviewApp.Repository
{
    public class PokemonRepository: IPokemonRepository
    {
        private DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int CategoryId, Pokemon pokemon)
        {
          
            var Pcategory = _context.Categories.Where(c=> c.Id == CategoryId).FirstOrDefault();
            var Powner = _context.Owners.Where(p => p.Id == ownerId).FirstOrDefault();

            var pokemonCategory = new PokemonCategory()
            {
                Category = Pcategory,
                Pokemon = pokemon
            };

            _context.PokemonCategories.Add(pokemonCategory);
            var pokemonOwner = new PokemonOwner()
            {
                Owner = Powner,
                Pokemon= pokemon
            };
            _context.PokemonOwners.Add(pokemonOwner);
    
            _context.Add(pokemon);
            _context.SaveChanges();
            
            return true;

        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            _context.SaveChanges();
            return true;
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.Where(p => p.Name == name).FirstOrDefault();
            
        }

        public decimal GetPokemonRating(int pokemonid)
        {
            var review = _context.Reviews.Where(p => p.Pokemon.Id == pokemonid);
            if (review.Count() <= 0)
                return 0;
            return ((decimal)review.Sum(r => r.Rating)/ review.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.ToList();
        }

        public bool PokemonExsits(int pokemonid)
        {
            return _context.Pokemons.Any(p => p.Id == pokemonid);
        }

        public bool UpdatePokemon(int ownerId, int CategoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);
            _context.SaveChanges();
            return true;
        }
    }
}
