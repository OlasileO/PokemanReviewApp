using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemanReviewApp.DTO;
using PokemanReviewApp.Interfaces;
using PokemanReviewApp.Model;

namespace PokemanReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetCountries()
        {
            var Countries = _mapper.Map<List<CountryDTO>>(_repository.GetCountries());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Countries);
        }

        [HttpGet("{countryid}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryid)
        {
            if (!_repository.CountryExist(countryid))
                return NotFound();
            var country = _mapper.Map<CountryDTO>(_repository.GetCountry(countryid));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }

        [HttpGet("owner/{ownerid}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryid(int ownerid)
        {
            var country = _mapper.Map<CountryDTO>(_repository.GetCountryOwner(ownerid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }
    }
}
