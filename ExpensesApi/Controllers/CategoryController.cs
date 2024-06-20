using AutoMapper;
using ExpensesApi.Interfaces;
using ExpensesApi.Models.Dtos;
using ExpensesApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryServices categoryServices, IMapper mapper)
        {
            _categoryServices = categoryServices;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> Get()
        {
            return _mapper.Map<IEnumerable<CategoryDto>>(await _categoryServices.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryServices.GetById(id);

            if (category == null)
                throw new KeyNotFoundException($"Category no se encontro");

            CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);

            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            Category category = _mapper.Map<Category>(categoryDto);
            await _categoryServices.Create(category);
            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryDto categoryDto)
        {
            if (categoryDto.Id != id)
                throw new KeyNotFoundException($"Id no coincide");

            var category = await _categoryServices.GetById(id);

            if (category == null)
                throw new KeyNotFoundException($"Category no se encontro");

            _mapper.Map(categoryDto, category);

            await _categoryServices.Update();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryToDelete = await _categoryServices.GetById(id);

            if (categoryToDelete == null)
                throw new KeyNotFoundException($"Category no se encontro");

            await _categoryServices.Delete(categoryToDelete);
            return Ok();
        }

    }
}
