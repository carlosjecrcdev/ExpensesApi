namespace ExpensesApi.Models.Dtos
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int? CatId { get; set; }
    }
}
