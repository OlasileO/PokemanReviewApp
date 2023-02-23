using PokemanReviewApp.Interfaces;
using PokemanReviewApp.Model;

namespace PokemanReviewApp.Repository
{
    public class OwnerRepository: IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            _context.SaveChanges();
            return true;
        }

        public Owner GetOwner(int id)
        {
            return _context.Owners.Where(o => o.Id == id).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeid)
        {
            return _context.PokemonOwners.Where(o=> o.Pokemon.Id == pokeid).Select(o=> o.Owner).ToList();   
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerid)
        {
            return _context.PokemonOwners.Where(o=> o.Owner.Id == ownerid).Select(o=> o.Pokemon).ToList();
        }

        public bool OwnerExist(int id)
        {
          return _context.Owners.Any(o=> o.Id == id);
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            _context.SaveChanges();
            return true;
        }
    }
}
