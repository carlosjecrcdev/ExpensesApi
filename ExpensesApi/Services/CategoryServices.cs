using ExpensesApi.Interfaces;
using ExpensesApi.Models.Context;
using ExpensesApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApi.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ExpensesContext _context;

        public CategoryServices(ExpensesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task Create(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
