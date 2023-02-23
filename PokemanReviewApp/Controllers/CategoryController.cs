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
        [ProducesResponseType(200 , Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int id)
        {
            if (!_category.CategoryExist(id))
                return NotFound();
            var category = _mapper.Map<CategoryDTO>(_category.GetCategory(id));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(category);
        }

        [HttpGet("pokemon/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryid(int id)
        {
          var pokemon = _mapper.Map<List<PokemonDTO>>(_category.GetPokemonByCategory(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }

    }
}
