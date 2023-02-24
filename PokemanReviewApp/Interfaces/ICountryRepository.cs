using PokemanReviewApp.Model;

namespace PokemanReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryOwner(int ownerid);
        ICollection<Owner> GetOwnerFromCountry(int countryid);
        bool CountryExist(int id);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
    }
}
