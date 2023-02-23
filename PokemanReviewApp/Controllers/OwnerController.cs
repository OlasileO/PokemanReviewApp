using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemanReviewApp.DTO;
using PokemanReviewApp.Interfaces;
using PokemanReviewApp.Model;
using PokemanReviewApp.Repository;

namespace PokemanReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwners()
        {
            var owner = _mapper.Map<List<OwnerDTO>>(_ownerRepository.GetOwners());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int id)
        {
            if (!_ownerRepository.OwnerExist(id))
                return BadRequest(ModelState);
            var category = _mapper.Map<Owner>(_ownerRepository.GetOwner(id));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(category);
        }

        [HttpGet("{ownerid}/pokmon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerid)
        {
            if (!_ownerRepository.OwnerExist(ownerid))
                return NotFound(ModelState);
            var owner = _mapper.Map<List<PokemonDTO>>
                (_ownerRepository.GetPokemonByOwner(ownerid));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryid, [FromBody] OwnerDTO createowner)
        {
            if (createowner == null)
                return BadRequest(ModelState);

            var owner = _ownerRepository.GetOwners().Where(o => o.Name.Trim().ToUpper() ==
            createowner.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (owner != null)
            {
                ModelState.AddModelError("", "Owner is Already Exist");
                return StatusCode(422, ModelState);

            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownermap = _mapper.Map<Owner>(createowner);
            ownermap.Country = _countryRepository.GetCountry(countryid);
            if (!_ownerRepository.CreateOwner(ownermap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");

        }

        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerid, [FromBody] OwnerDTO updateOwner)
        {
            if (updateOwner == null)
                return BadRequest(ModelState);

            if (ownerid != updateOwner.Id)
                return BadRequest(ModelState);

            if (!_ownerRepository.OwnerExist(ownerid))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownermap = _mapper.Map<Owner>(updateOwner);

            if (!_ownerRepository.UpdateOwner(ownermap))
            {
                ModelState.AddModelError("", "Something went wrong when updating");

                return StatusCode(500, ModelState);

            }
            return NoContent();
        }


        [HttpDelete]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerid)
        {
            if (!_ownerRepository.OwnerExist(ownerid))
                return NotFound();
            var ownerDelete = _ownerRepository.GetOwner(ownerid);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_ownerRepository.DeleteOwner(ownerDelete))
            {
                ModelState.AddModelError("", "Something went wrong");

            }
            return NoContent();
        }
    }
}
