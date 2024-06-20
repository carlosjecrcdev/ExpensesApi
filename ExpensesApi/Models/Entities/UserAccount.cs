using System.ComponentModel.DataAnnotations;

namespace ExpensesApi.Models.Entities
{
    public partial class UserAccount
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsAdmin { get; set; }
    }
}
