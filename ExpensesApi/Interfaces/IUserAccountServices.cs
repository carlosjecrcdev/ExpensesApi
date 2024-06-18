using ExpensesApi.Models;

namespace ExpensesApi.Interfaces
{
    public interface IUserAccountServices
    {
        /// <summary>
        /// Get All UserAccounts
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserAccount>> GetAll();
        /// <summary>
        /// Get UserAccount By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserAccount?> GetById(int id);
        /// <summary>
        /// Create new UserAccouunt
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        Task Create(UserAccount userAccount);
        /// <summary>
        /// Update UserAccount
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        Task Update(UserAccount userAccount);
        /// <summary>
        /// Delete UserAccount
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);
    }
}
