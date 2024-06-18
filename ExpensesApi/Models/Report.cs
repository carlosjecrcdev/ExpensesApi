using System;
using System.Collections.Generic;

namespace ExpensesApi.Models
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime GeneratedDate { get; set; }
        public string? Content { get; set; }
    }
}
