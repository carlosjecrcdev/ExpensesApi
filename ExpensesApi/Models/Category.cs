using System;
using System.Collections.Generic;

namespace ExpensesApi.Models
{
    public partial class Category
    {
        public Category()
        {
            Expenses = new HashSet<Expense>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
