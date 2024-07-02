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
        public async Task<ActionResult<ApiResponse<IEnumerable<BudgetDto>>>> Get()
        {
            var budgets = _mapper.Map<IEnumerable<BudgetDto>>(await _budgetServices.GetAll());

            return Ok(new ApiResponse<IEnumerable<BudgetDto>>
            {
                Success = true,
                Data = budgets
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BudgetDto>>> GetById(int id)
        {
            var budget = await _budgetServices.GetById(id)
                ?? throw new KeyNotFoundException("Budget not found");

            var budgetDto = _mapper.Map<BudgetDto>(budget);

            return Ok(new ApiResponse<BudgetDto>
            {
                Success = true,
                Data = budgetDto
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<BudgetDto>>> Create(BudgetDto budgetDto)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                throw new ApiExceptions(messages);
            }

            var budget = _mapper.Map<Budget>(budgetDto);
            await _budgetServices.Create(budget);

            return CreatedAtAction(nameof(GetById), new { id = budget.BudgetId }, new ApiResponse<BudgetDto>
            {
                Success = true,
                Data = _mapper.Map<BudgetDto>(budget)
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BudgetDto budgetDto)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                throw new ApiExceptions(messages);
            }

            if (budgetDto.Id != id)
                throw new KeyNotFoundException("Id is different");

            var budget = await _budgetServices.GetById(id)
                ?? throw new KeyNotFoundException("Budget not found");

            _mapper.Map(budgetDto, budget);

            await _budgetServices.Update();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            var budgetToDelete = await _budgetServices.GetById(id)
                ?? throw new KeyNotFoundException("Budget not found");

            await _budgetServices.Delete(budgetToDelete);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = "Budget deleted successfully"
            });
        }
    }
}
