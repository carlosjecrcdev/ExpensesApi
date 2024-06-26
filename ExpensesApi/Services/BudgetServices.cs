using ExpensesApi.Interfaces;
using ExpensesApi.Models.Context;
using ExpensesApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApi.Services
{
    public class BudgetServices : IBudgetServices
    {
        private readonly ExpensesContext _context;

        public BudgetServices(ExpensesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Budget>> GetAll()
        {
            return await _context.Budgets.ToListAsync();
        }

        public async Task<Budget?> GetById(int id)
        {
            return await _context.Budgets.FindAsync(id);
        }

        public async Task Create(Budget budget)
        {
            await _context.Budgets.AddAsync(budget);
            await _context.SaveChangesAsync();
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Budget budget)
        {
            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
        }       
    }
}
