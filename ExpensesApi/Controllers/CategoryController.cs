using AutoMapper;
using ExpensesApi.Exceptions;
using ExpensesApi.Interfaces;
using ExpensesApi.Models.Dtos;
using ExpensesApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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
            var category = await _categoryServices.GetById(id)
                ?? throw new KeyNotFoundException($"Category not found");

            CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);

            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                throw new ApiExceptions(messages);
            }
            Category category = _mapper.Map<Category>(categoryDto);
            await _categoryServices.Create(category);
            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                throw new ApiExceptions(messages);
            }
            if (categoryDto.Id != id)
                throw new KeyNotFoundException($"Id is diferent");

            var category = await _categoryServices.GetById(id)
                ?? throw new KeyNotFoundException($"Category not found");

            _mapper.Map(categoryDto, category);

            await _categoryServices.Update();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryToDelete = await _categoryServices.GetById(id)
                ?? throw new KeyNotFoundException($"Category not found");

            await _categoryServices.Delete(categoryToDelete);
            return Ok();
        }

    }
}
