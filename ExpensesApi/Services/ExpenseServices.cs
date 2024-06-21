using ExpensesApi.Interfaces;
using ExpensesApi.Models.Context;
using ExpensesApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApi.Services
{
    public class ExpenseServices : IExpenseServices
    {
        private readonly ExpensesContext _context;

        public ExpenseServices(ExpensesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetAll()
        {
            return await _context.Expenses.ToListAsync();
        }

        public async Task<Expense?> GetById(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }

        public async Task Create(Expense expense)
        { 
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Expense expense)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
    }
}
