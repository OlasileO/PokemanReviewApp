using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using PokemanReviewApp.DTO;
using PokemanReviewApp.Interfaces;
using PokemanReviewApp.Model;
using PokemanReviewApp.Repository;

namespace PokemanReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers()
        {
            var reviewer = _mapper.Map<List<ReviewDTO>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok (reviewer);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int id)
        {
            if (!_reviewerRepository.ReviewerExist(id))
                return BadRequest(ModelState);
            var review =_mapper.Map<ReviewerDTO>( _reviewerRepository.GetReviewer(id));
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(review);
        }

        [HttpGet("{reviewerid}/review")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviwByReviewer(int reviewerid)
        {
            if(_reviewerRepository.ReviewerExist(reviewerid))
                return NotFound();

            var reviewer = _mapper.Map<List<ReviewDTO>>
                (_reviewerRepository.GetReviwByAReviewer(reviewerid));

            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(reviewer);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDTO reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);
            var review = _reviewerRepository.GetReviewers()
                .Where(r => r.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (review != null)
            {
                ModelState.AddModelError("", "Reviewer  already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);
            
            if (!_reviewerRepository.CreateReviwer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO UpdateReviewer)
        {
            if (UpdateReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != UpdateReviewer.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewerMap = _mapper.Map<Reviewer>(UpdateReviewer);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return BadRequest(ModelState);

            var reviewerDelete = _reviewerRepository.GetReviewer(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewerRepository.DeleteReviewer(reviewerDelete))
            {
                ModelState.AddModelError("", "Something Went wrong while Delete the Review");

            }
            return NoContent();
        }
    }
}
