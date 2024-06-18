using System.ComponentModel.DataAnnotations;

namespace ExpensesApi.Models
{
    public partial class UserAccount
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsAdmin { get; set; }
    }
}
