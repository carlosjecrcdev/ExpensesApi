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
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseServices _expenseServices;
        private readonly IMapper _mapper;

        public ExpenseController(IExpenseServices expenseServices, IMapper mapper)
        {
            _expenseServices = expenseServices;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ExpenseDto>> Get()
        {
            return _mapper.Map<IEnumerable<ExpenseDto>>(await _expenseServices.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> GetById(int id)
        {
            var expense = await _expenseServices.GetById(id)
                ?? throw new KeyNotFoundException($"Expense not found");

            BudgetDto expenseDto = _mapper.Map<BudgetDto>(expense);

            return Ok(expenseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseDto expenseDto)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                throw new ApiExceptions(messages);
            }
            Expense expense = _mapper.Map<Expense>(expenseDto);
            await _expenseServices.Create(expense);
            return CreatedAtAction(nameof(GetById), new { id = expense.ExpenseId }, expense);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ExpenseDto expenseDto)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                throw new ApiExceptions(messages);
            }
            if (expenseDto.Id != id)
                throw new KeyNotFoundException($"Id is diferent");

            var expense = await _expenseServices.GetById(id)
                ?? throw new KeyNotFoundException($"Expense not found");

            _mapper.Map(expenseDto, expense);

            await _expenseServices.Update();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expenseToDelete = await _expenseServices.GetById(id)
                ?? throw new KeyNotFoundException($"Expense not found");

            await _expenseServices.Delete(expenseToDelete);
            return Ok();
        }

    }
}
