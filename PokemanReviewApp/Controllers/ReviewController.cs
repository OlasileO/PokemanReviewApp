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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokeRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper,
            IPokemonRepository pokeRepository, IReviewerRepository reviewerRepository)

        {
    
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokeRepository = pokeRepository;
            _reviewerRepository = reviewerRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            var review = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int id)
        {
            if (!_reviewRepository.ReviewExists(id))
                return BadRequest(ModelState);
            var review =_mapper.Map<ReviewDTO>( _reviewRepository.GetReview(id));
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(review);
        }

        [HttpGet("pokemon/{pokeid}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewOfPokemon(int pokeid)
        {
            var pokemonReview = _mapper.Map<List<ReviewDTO>>
                (_reviewRepository.GetAllReviewOfPokemon(pokeid));

            if(!ModelState.IsValid)
                return BadRequest();
            return Ok(pokemonReview);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerid, [FromQuery]int pokeid, [FromBody] ReviewDTO reviewCreate)
        {
            if(reviewCreate == null)
                return BadRequest(ModelState);
            var review = _reviewRepository.GetReviews()
                .Where(r => r.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpperInvariant())
                .FirstOrDefault();
            if(review == null)
            {
                ModelState.AddModelError("", "Review  already exist");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.Pokemon = _pokeRepository.GetPokemon(pokeid);
            reviewMap.Reviewer =_reviewerRepository.GetReviewer(reviewerid);
            if (_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDTO UpdateReview)
        {
            if(UpdateReview== null) 
                return BadRequest(ModelState);

            if(reviewId != UpdateReview.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewMap = _mapper.Map<Review>(UpdateReview);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId)) 
                return BadRequest(ModelState);

            var reviewDelete = _reviewRepository.GetReview(reviewId);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_reviewRepository.DeleteReview(reviewDelete))
            {
                ModelState.AddModelError("", "Something Went wrong while Delete the Review");
                
            }
            return NoContent();
        }
    }
}
