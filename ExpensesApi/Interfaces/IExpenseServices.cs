using ExpensesApi.Models.Entities;

namespace ExpensesApi.Interfaces
{
    public interface IExpenseServices
    {
        /// <summary>
        /// Get All Budget
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Expense>> GetAll();
        /// <summary>
        /// Get Expense By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Expense?> GetById(int id);
        /// <summary>
        /// Create new Expense
        /// </summary>
        /// <param name="expense"></param>
        /// <returns></returns>
        Task Create(Expense expense);
        /// <summary>
        /// Update Expense
        /// </summary>
        /// <returns></returns>
        Task Update();
        /// <summary>
        /// Delete Budget
        /// </summary>
        /// <param name="expense"></param>
        /// <returns></returns>
        Task Delete(Expense expense);
    }
}
