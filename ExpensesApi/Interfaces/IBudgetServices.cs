using ExpensesApi.Models.Dtos;
using ExpensesApi.Models.Entities;

namespace ExpensesApi.Interfaces
{
    public interface IBudgetServices
    {
        /// <summary>
        /// Get All Budget
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Budget>> GetAll();
        /// <summary>
        /// Get Budget By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Budget?> GetById(int id);
        /// <summary>
        /// Create new Budget
        /// </summary>
        /// <param name="budget"></param>
        /// <returns></returns>
        Task Create(Budget budget);
        /// <summary>
        /// Update Budget
        /// </summary>
        /// <returns></returns>
        Task Update();
        /// <summary>
        /// Delete Budget
        /// </summary>
        /// <param name="budget"></param>
        /// <returns></returns>
        Task Delete(Budget budget);
    }
}
