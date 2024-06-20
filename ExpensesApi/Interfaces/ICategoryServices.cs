using ExpensesApi.Models.Entities;

namespace ExpensesApi.Interfaces
{
    public interface ICategoryServices
    {
        /// <summary>
        /// Get All Category
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Category>> GetAll();
        /// <summary>
        /// Get Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Category?> GetById(int id);
        /// <summary>
        /// Create new Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task Create(Category category);
        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task Update();
        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task Delete(Category category);
    }
}
