using ExpensesApi.Interfaces;
using ExpensesApi.Models.Context;
using ExpensesApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace ExpensesApi.Services
{
    public class UserAccountServices: IUserAccountServices
    {
        private readonly ExpensesContext _context;
        public UserAccountServices(ExpensesContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<UserAccount>> GetAll()
        {
            return await _context.UserAccounts.ToListAsync();
        }

        public async Task<UserAccount?> GetById(int id)
        {
            return await _context.UserAccounts.FindAsync(id);
        }

        public async Task Create(UserAccount userAccount)
        {         
            await _context.UserAccounts.AddAsync(userAccount);
            await _context.SaveChangesAsync();
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(UserAccount userAccount)
        {
            _context.UserAccounts.Remove(userAccount);
            await _context.SaveChangesAsync();
        }
    }
}
