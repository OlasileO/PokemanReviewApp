using PokemanReviewApp.Model;

namespace PokemanReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int id);
        ICollection<Owner> GetOwnerOfAPokemon( int pokeid);
        ICollection<Pokemon> GetPokemonByOwner(int ownerid);
       
        bool OwnerExist(int id);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);

    }
}
