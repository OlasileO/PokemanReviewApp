using PokemanReviewApp.Model;

namespace PokemanReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        decimal GetPokemonRating(int pokemonid);
        bool PokemonExsits(int pokemonid);
        bool CreatePokemon(int ownerId, int CategoryId, Pokemon pokemon);
        bool UpdatePokemon(int ownerId, int CategoryId, Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
    }
}
