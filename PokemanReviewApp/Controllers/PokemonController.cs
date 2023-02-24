using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemanReviewApp.DTO;
using PokemanReviewApp.Interfaces;
using PokemanReviewApp.Model;

namespace PokemanReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _repository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository repository,IMapper mapper, IReviewRepository reviewRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons =_mapper.Map<List<PokemonDTO>>( _repository.GetPokemons());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type =typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int id)
        {
            if (!_repository.PokemonExsits(id))
                return NotFound();
            var pokemon = _mapper.Map<PokemonDTO>(_repository.GetPokemon(id));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int id)
        {
            if (!_repository.PokemonExsits(id))
                return NotFound();

            var rating = _repository.GetPokemonRating(id);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(rating);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromQuery]int ownerId, [FromQuery]int catId,[FromBody] PokemonDTO pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);
            var pokemon = _repository.GetPokemons()
                .Where(r => r.Name.Trim().ToUpper() ==pokemonCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (pokemon != null)
            {
                ModelState.AddModelError("", "Review  already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var PokeMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_repository.CreatePokemon( ownerId,catId,PokeMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");
        }
        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int pokeId, [FromQuery] int ownerId,
            [FromQuery] int catId, [FromBody] PokemonDTO pokemonUpdate)
        {
            if (pokemonUpdate == null)
                return BadRequest(ModelState);

            if (pokeId != pokemonUpdate.Id)
                return BadRequest(ModelState);

            if (!_repository.PokemonExsits(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var pokeMap = _mapper.Map<Pokemon>(pokemonUpdate);

            if (!_repository.UpdatePokemon(ownerId,catId,pokeMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_repository.PokemonExsits(pokeId))
                return BadRequest(ModelState);

            var reviewToDelete =_reviewRepository.GetAllReviewOfPokemon(pokeId);
            var pokmon = _repository.GetPokemon(pokeId);

            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewRepository.DeleteReviews(reviewToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
            }

            if (!_repository.DeletePokemon(pokmon))
            {
                ModelState.AddModelError("", "Something Went wrong while Delete the Review");

            }
            return NoContent();
        }
    }
}
