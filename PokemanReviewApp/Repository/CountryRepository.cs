﻿using PokemanReviewApp.Interfaces;
using PokemanReviewApp.Model;

namespace PokemanReviewApp.Repository
{
    public class CountryRepository: ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CountryExist(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteCountry(Country country)
        {
           _context.Remove(country);
            _context.SaveChanges();
            return true;
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryOwner(int ownerid)
        {
            return _context.Owners.Where(o => o.Id == ownerid).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerFromCountry(int countryid)
        {
           return _context.Owners.Where(o => o.Country.Id == countryid).ToList();
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            _context.SaveChanges();
            return true;
        }
    }
}
