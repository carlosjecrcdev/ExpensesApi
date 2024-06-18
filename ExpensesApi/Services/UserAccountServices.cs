using ExpensesApi.Interfaces;
using ExpensesApi.Models;
using ExpensesApi.Models.Context;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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

        public async Task Update(UserAccount userAccount)
        {
            _context.UserAccounts.Update(userAccount);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var account = await _context.UserAccounts.FindAsync(id);

            if (account != null)
            {
                _context.UserAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }
}
