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
        public async Task<IEnumerable<Expense>> Get()
        {
            return _mapper.Map<IEnumerable<Expense>>(await _expenseServices.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetById(int id)
        {
            var expense = await _expenseServices.GetById(id);

            if (expense == null)
                throw new KeyNotFoundException($"Expense no se encontro");

            ExpenseDto expenseDto = _mapper.Map<ExpenseDto>(expense);

            return Ok(expenseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseDto expenseDto)
        {
            Expense expense = _mapper.Map<Expense>(expenseDto);
            await _expenseServices.Create(expense);
            return CreatedAtAction(nameof(GetById), new { id = expense.ExpenseId }, expense);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ExpenseDto expenseDto)
        {
            if (expenseDto.Id != id)
                throw new KeyNotFoundException($"Id no coincide");

            var expense = await _expenseServices.GetById(id);

            if (expense == null)
                throw new KeyNotFoundException($"Expense no se encontro");

            _mapper.Map(expenseDto, expense);

            await _expenseServices.Update();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expenseToDelete = await _expenseServices.GetById(id);

            if (expenseToDelete == null)
                throw new KeyNotFoundException($"Expense no se encontro");

            await _expenseServices.Delete(expenseToDelete);
            return Ok();
        }

    }
}
