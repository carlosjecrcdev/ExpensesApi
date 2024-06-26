using ExpensesApi.Models.Dtos;
using FluentValidation;

namespace ExpensesApi.Models.Validations
{
    /// <summary>
    /// Validation Entity Class
    /// </summary>
    public class UserAccountValidator : AbstractValidator<UserAccountDto>
    {
        public UserAccountValidator()
        {
            RuleFor(e => e.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(e => e.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(e => e.Email).EmailAddress().WithMessage("Email is incorrect");
            RuleFor(e => e.FirstName).NotEmpty().WithMessage("First Name is required");
            RuleFor(e => e.LastName).NotEmpty().WithMessage("Last Name is required");
            RuleFor(e => e.Admin).NotEmpty().WithMessage("Admin is required");
        }
    }
}
