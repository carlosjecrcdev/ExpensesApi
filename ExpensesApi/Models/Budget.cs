using System;
using System.Collections.Generic;

namespace ExpensesApi.Models
{
    public partial class Budget
    {
        public int BudgetId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
