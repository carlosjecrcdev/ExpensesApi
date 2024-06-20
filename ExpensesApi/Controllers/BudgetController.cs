using AutoMapper;
using ExpensesApi.Interfaces;
using ExpensesApi.Models.Dtos;
using ExpensesApi.Models.Entities;
using ExpensesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetServices _budgetServices;
        private readonly IMapper _mapper;

        public BudgetController(IBudgetServices budgetServices, IMapper  mapper )
        {
            _budgetServices = budgetServices;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<BudgetDto>> Get()
        {
            return _mapper.Map<IEnumerable<BudgetDto>>(await _budgetServices.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetDto>> GetById(int id)
        {
            var budget = await _budgetServices.GetById(id);

            if (budget == null)
                throw new KeyNotFoundException($"Budget no se encontro");

            BudgetDto userDto = _mapper.Map<BudgetDto>(budget);

            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BudgetDto budgetDto)
        {
            Budget budget = _mapper.Map<Budget>(budgetDto);
            await _budgetServices.Create(budget);
            return CreatedAtAction(nameof(GetById), new { id = budget.BudgetId }, budget);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BudgetDto budgetDto)
        {
            if (budgetDto.Id != id)
                throw new KeyNotFoundException($"Id no coincide");

            var budget = await _budgetServices.GetById(id);

            if (budget == null)
                throw new KeyNotFoundException($"Budget no se encontro");

            _mapper.Map(budgetDto, budget);

            await _budgetServices.Update();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var budgetToDelete = await _budgetServices.GetById(id);

            if (budgetToDelete == null)
                throw new KeyNotFoundException($"Budget no se encontro");

            await _budgetServices.Delete(budgetToDelete);
            return Ok();
        }

    }
}
