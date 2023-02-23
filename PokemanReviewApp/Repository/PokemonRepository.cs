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
    }
}
