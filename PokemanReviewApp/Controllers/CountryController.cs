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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDTO CountryCreate)
        {
            if (CountryCreate == null)
                return BadRequest(ModelState);
            var category = _repository.GetCountries()
                .Where(r => r.Name.Trim().ToUpper() == CountryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("", "Review  already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(CountryCreate);

            if (!_repository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");
        }
        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int countryId, [FromBody] CountryDTO UpdateCountry)
        {
            if (UpdateCountry == null)
                return BadRequest(ModelState);

            if (countryId != UpdateCountry.Id)
                return BadRequest(ModelState);

            if (!_repository.CountryExist(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var countryMap = _mapper.Map<Country>(UpdateCountry);

            if (!_repository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_repository.CountryExist(countryId))
                return BadRequest(ModelState);

            var countryDelete = _repository.GetCountry(countryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_repository.DeleteCountry(countryDelete))
            {
                ModelState.AddModelError("", "Something Went wrong while Delete the Review");

            }
            return NoContent();
        }
    }
}
