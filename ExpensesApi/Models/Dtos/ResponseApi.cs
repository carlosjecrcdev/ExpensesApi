namespace ExpensesApi.Models.Dtos
{
    public class ApiResponse<T>
    {
        /// <summary>
        /// Property success
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Property Messgae
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Property Data
        /// </summary>
        public T? Data { get; set; }
    }
}
