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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _category;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository category, IMapper mapper)
        {
            _category = category;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategories()
        {
            var category = _mapper.Map<List<CategoryDTO>>(_category.GetCategories());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(category);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int id)
        {
            if (!_category.CategoryExist(id))
                return NotFound();
            var category = _mapper.Map<CategoryDTO>(_category.GetCategory(id));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(category);
        }

        [HttpGet("pokemon/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryid(int id)
        {
            var pokemon = _mapper.Map<List<PokemonDTO>>(_category.GetPokemonByCategory(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDTO CategoryCreate)
        {
            if (CategoryCreate == null)
                return BadRequest(ModelState);
            var category = _category.GetCategories()
                .Where(r => r.Name.Trim().ToUpper() == CategoryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("", "Review  already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(CategoryCreate);

            if (!_category.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");
        }
        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDTO UpdateCategory)
        {
            if (UpdateCategory == null)
                return BadRequest(ModelState);

            if (categoryId != UpdateCategory.Id)
                return BadRequest(ModelState);

            if (!_category.CategoryExist(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryMap = _mapper.Map<Category>(UpdateCategory);

            if (!_category.UpadateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_category.CategoryExist(categoryId))
                return BadRequest(ModelState);

            var categoryDelete = _category.GetCategory(categoryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_category.DeleteCategory(categoryDelete))
            {
                ModelState.AddModelError("", "Something Went wrong while Delete the Review");

            }
            return NoContent();
        }
    }
}
