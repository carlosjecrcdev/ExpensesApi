using System;
using System.Collections.Generic;

namespace ExpensesApi.Models
{
    public partial class Expense
    {
        public int ExpenseId { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
    }
}
